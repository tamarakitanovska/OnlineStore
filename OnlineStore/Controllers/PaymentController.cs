using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OnlineStore.Models;


namespace OnlineStore.Controllers
{
   

    public class PaymentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private static MyWallet myWallet = new MyWallet("32ed97a8-9782-4cca-b50b-ad3e2917143e","23081997aab");
        private static HttpClient client = new HttpClient();        
        private static String baseURL = "http://127.0.0.1:3000/";
        
        private static String ControllerNameForPay="merchant/";
        private static String BaseWebAddressSimpleApi = "https://blockchain.info/q/";
        private static HttpClient clientSimpleApi = new HttpClient();
        private static HttpClient ClientForPriceConvert = new HttpClient();
        private static String URLforPriceConverting = "https://blockchain.info/tobtc?";

        static async Task RunAsync()
        {
            if(client.BaseAddress==null)
                client.BaseAddress = new Uri(baseURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            clientSimpleApi.BaseAddress = new Uri(BaseWebAddressSimpleApi);
            clientSimpleApi.DefaultRequestHeaders.Accept.Clear();
            clientSimpleApi.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        static async Task<AddressForCustumer> GetNewAddressAsync(String path)
        {
            AddressForCustumer addressForCustumer = null;

            HttpResponseMessage httpResponseMessage = await client.GetAsync(ControllerNameForPay+path).ConfigureAwait(false);

            if(httpResponseMessage.IsSuccessStatusCode)
            {
                addressForCustumer = await httpResponseMessage.Content.ReadAsAsync<AddressForCustumer>();
            }

            return addressForCustumer;
        }

        /// <summary>
        /// Get ballance of an address wiht the number of confirmations
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static async Task<BallanceOfAddress> GetBalanceOfAddressWithConfirmations(String path)
        {
            //prakjanje na http baranje i prevzimanje na podatoci vo vrska so saldoto na konkretnata adresa
            BallanceOfAddress ballance = new BallanceOfAddress();
            HttpResponseMessage httpResponseMessage = await clientSimpleApi.GetAsync(path).ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                ballance.Balance= Convert.ToInt32(await httpResponseMessage.Content.ReadAsStringAsync());
            }
            return ballance;
        }

        
        static async Task<BallanceOfAddress> GetBalanceOfAddress(String path)
        {
            //prakjanje na http baranje i prevzimanje na podatoci vo vrska so saldoto na konkretnata adresa
            BallanceOfAddress ballance = null;
            HttpResponseMessage httpResponseMessage = await client.GetAsync(ControllerNameForPay + path).ConfigureAwait(false);
            if(httpResponseMessage.IsSuccessStatusCode)
            {
                ballance =await httpResponseMessage.Content.ReadAsAsync<BallanceOfAddress>();
            }
            return ballance;
        }


        // 

        public async Task<ActionResult> Pay(int ID)
        {
            //Saving the info for the user who request payment and his cart
            ShoppingCart shoppingCart=db.ShoppingCarts.Find(ID);
            
            await RunAsync();
            return View(new UserBitcoinAdress(shoppingCart));
        }

        public async Task<Double> PriceOfBitcoin(double ammount,String currency)
        {
            //sending http request using ClinetForPriceConvert client and 
            //receiving httpResponseMessage
            HttpResponseMessage httpResponseMessage = await ClientForPriceConvert.GetAsync(URLforPriceConverting + "currency=" + 
                currency + "&value=" + ammount.ToString()).ConfigureAwait(false);
            if(httpResponseMessage.IsSuccessStatusCode)
            {
                return Convert.ToDouble(httpResponseMessage.Content.ReadAsStringAsync().Result);
            }
            return -1;
           
        }

        private async Task<AddressForCustumer> getNewAddressFromApiAsync(UserBitcoinAdress model)
        {
            String path = myWallet.ID + "/new_address?password=" + myWallet.Password + "&label=" + model.UserId;
            //needs validation
            
            AddressForCustumer address = await GetNewAddressAsync(path);
            return address;
            
        }
        

        /// <summary>
        /// Generate new address for receiving bitcoin for every
        /// new custumer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GenerateAddressAsync(UserBitcoinAdress model)
        {
            //Generiranje na nova adresa za sekoj nov korisnik
            AddressForCustumer newAddress =await getNewAddressFromApiAsync(model);
            //model for all information needed for payment
            ReceivingPaymentForUser receivingPaymentForUser = new ReceivingPaymentForUser();
            //the generated address for the specific user
            receivingPaymentForUser.AddressForReceiving = newAddress.NewAddress;
            //the label used when generating address
            receivingPaymentForUser.Label = newAddress.Label;
            //the bitcoin address of the specific usre
            receivingPaymentForUser.UserAddress = model.UserAddress;
            //the id of the specific user
            receivingPaymentForUser.UserId = model.UserId;
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(model.ShoppingCartId);
            //the shopping cart id and the ammount to be paid
            receivingPaymentForUser.ShoppingCartId = shoppingCart.ID;
            receivingPaymentForUser.ammountToBePaid = shoppingCart.toBePaid();
            receivingPaymentForUser.ammountToBePaidBitcoin = PriceOfBitcoin(receivingPaymentForUser.ammountToBePaid, "USD").Result;
            return View(receivingPaymentForUser);
        }

        //Saldo na za soodvetna adresa bez broj na potvrdi
        private BallanceOfAddress getBalanceNoConfirmations(String path)
        {
            return GetBalanceOfAddress(path).Result;
        }
        
        /// <summary>
        /// Geting the number of confirmations
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public async Task<ActionResult> StatusAsync(String address)
        {
            //Proverka na statusot na transakcijata 
            //go proveruvame saldoto za ovaa adresa
            StatusOfTransaction statusOfTransaction = new StatusOfTransaction();
            statusOfTransaction.AddressForPayment = address;
            //saldo so 0 potvrdi
            String path = "addressbalance/" + address + "?confirmations=0";
            BallanceOfAddress ballance = await GetBalanceOfAddressWithConfirmations(path);
            if (ballance.getBalanceAsEuro() == 0)
            {
                statusOfTransaction.Balance = ballance;
                statusOfTransaction.BitcoinsSent = false;
                statusOfTransaction.BlockConfirmations = 0;
                return View(statusOfTransaction);
            }
            //saldo so 1 potvrda
            path = "addressbalance/" + address + "?confirmations=1";
            ballance = await GetBalanceOfAddressWithConfirmations(path);
            if (ballance.getBalanceAsEuro() == 0)
            {
                statusOfTransaction.Balance = ballance;
                statusOfTransaction.BitcoinsSent = true;
                statusOfTransaction.BlockConfirmations = 0;
                return View(statusOfTransaction);
            }
            //saldo so 2 potvrdi
            path = "addressbalance/" + address + "?confirmations=2";
            ballance = await GetBalanceOfAddressWithConfirmations(path);
            if (ballance.getBalanceAsEuro() == 0)
            {
                statusOfTransaction.Balance = ballance;
                statusOfTransaction.BitcoinsSent = true;
                statusOfTransaction.BlockConfirmations = 1;
                return View(statusOfTransaction);
            }
            //saldo so 3 potvrdi
            path = "addressbalance/" + address + "?confirmations=3";
            ballance = await GetBalanceOfAddressWithConfirmations(path);
            if (ballance.getBalanceAsEuro() == 0)
            {
                statusOfTransaction.Balance = ballance;
                statusOfTransaction.BitcoinsSent = true;
                statusOfTransaction.BlockConfirmations = 2;
                return View(statusOfTransaction);
            }
            statusOfTransaction.Balance = ballance;
            statusOfTransaction.BitcoinsSent = true;
            statusOfTransaction.BlockConfirmations = 3;
            return View(statusOfTransaction);
        }
    }
}