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
    [Route("api/AmountTransfer")]
    [ApiController]
    public class AmountTransferController : ControllerBase
    {

        [HttpGet]
        public ActionResult<List<AmountTransfer>> GetAll()
        {
            return TransferList._amtList;
        }


        [HttpPost]
        public ActionResult<List<AmountTransfer>> Create(AmountTransfer amtTransfer)
        {
            TransferList._amtList.Add(amtTransfer);
            return TransferList._amtList;
        }
    }
}