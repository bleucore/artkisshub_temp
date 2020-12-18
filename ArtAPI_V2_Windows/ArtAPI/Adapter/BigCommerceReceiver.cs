using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ArtAPI.Adapter
{
    public  class BigCommerceReceiver : ApiController, IWebHookReceiver
    {
        public string Name => throw new NotImplementedException();

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        //[HttpPost]
        //[Route("api/webooks/include/productcreate")]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        [HttpPost]
        [Route("api/webooks/include/productdelete")]
        public Task<HttpResponseMessage> ReceiveAsync(string id, HttpRequestContext context, HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }
    }
}
