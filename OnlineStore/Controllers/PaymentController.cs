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
        
        private static MyWallet myWallet = new MyWallet("32ed97a8-9782-4cca-b50b-ad3e2917143e","23081997aab");
        private static HttpClient client = new HttpClient();
        private static String baseURL = "http://127.0.0.1:3000/";
        private static String ControllerNameForPay="merchant/";
        private static String BaseWebAddressSimpleApi = "https://blockchain.info/q/";
        private static HttpClient clientSimpleApi = new HttpClient();


        static async Task RunAsync()
        {
            client.BaseAddress=new Uri(baseURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            clientSimpleApi.BaseAddress = new Uri(BaseWebAddressSimpleApi);
            clientSimpleApi.DefaultRequestHeaders.Accept.Clear();
            clientSimpleApi.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        static async Task<AddressForCustumer> GetNewAddress(String path)
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

        public async Task<ActionResult> Pay()
        {
            await RunAsync();
            return View();
        }

        private AddressForCustumer getNewAddressFromApi(UserBitcoinAdress model)
        {
            String path = myWallet.ID + "/new_address?password=" + myWallet.Password + "&label=" + model.UserId;
            //needs validation
            AddressForCustumer address = GetNewAddress(path).Result;
            return address;
            
        }
        

        /// <summary>
        /// Generate new address for receiving bitcoin for every
        /// new custumer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerateAddress(UserBitcoinAdress model)
        {
            //Generiranje na nova adresa za sekoj nov korisnik
            AddressForCustumer newAddress = getNewAddressFromApi(model);
            ReceivingPaymentForUser receivingPaymentForUser = new ReceivingPaymentForUser();
            receivingPaymentForUser.AddressForReceiving = newAddress.NewAddress;
            receivingPaymentForUser.Label = newAddress.Label;
            receivingPaymentForUser.UserAddress = model.UserAddress;
            receivingPaymentForUser.UserId = model.UserId;
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
        public ActionResult Status(String address)
        {
            //Proverka na statusot na transakcijata 
            //go proveruvame saldoto za ovaa adresa
            StatusOfTransaction statusOfTransaction = new StatusOfTransaction();
            statusOfTransaction.AddressForPayment = address;
            //saldo so 0 potvrdi
            String path = "addressbalance/" + address + "?confirmations=0";
            BallanceOfAddress ballance = GetBalanceOfAddressWithConfirmations(path).Result;
            if (ballance.getBalanceAsEuro() == 0)
            {
                statusOfTransaction.Balance = ballance;
                statusOfTransaction.BitcoinsSent = false;
                statusOfTransaction.BlockConfirmations = 0;
                return View(statusOfTransaction);
            }
            //saldo so 1 potvrda
            path = "addressbalance/" + address + "?confirmations=1";
            ballance = GetBalanceOfAddressWithConfirmations(path).Result;
            if (ballance.getBalanceAsEuro() == 0)
            {
                statusOfTransaction.Balance = ballance;
                statusOfTransaction.BitcoinsSent = true;
                statusOfTransaction.BlockConfirmations = 0;
                return View(statusOfTransaction);
            }
            //saldo so 2 potvrdi
            path = "addressbalance/" + address + "?confirmations=2";
            ballance = GetBalanceOfAddressWithConfirmations(path).Result;
            if (ballance.getBalanceAsEuro() == 0)
            {
                statusOfTransaction.Balance = ballance;
                statusOfTransaction.BitcoinsSent = true;
                statusOfTransaction.BlockConfirmations = 1;
                return View(statusOfTransaction);
            }
            //saldo so 3 potvrdi
            path = "addressbalance/" + address + "?confirmations=3";
            ballance = GetBalanceOfAddressWithConfirmations(path).Result;
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