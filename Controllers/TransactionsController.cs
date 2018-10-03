using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using WebAPIServiceII.Models;

namespace WebAPIServiceII.Controllers
{
    [Route("api/Transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {

        [HttpGet]
        public ActionResult<List<Transactions>> GetAll()
        {
            DBUtils du = new DBUtils();

            return du.getTransactions();
        }


        [HttpPost]
        public ActionResult<Transactions> Create(Transactions trans)
        {
            DBUtils du = new DBUtils();
            //Random generator = new Random();
            //string TransNumber = "DU" + generator.Next(0, 99999);
            //trans.TransactionNumber = TransNumber;

            return du.UpdateTransactions(trans);
        }
    }
}