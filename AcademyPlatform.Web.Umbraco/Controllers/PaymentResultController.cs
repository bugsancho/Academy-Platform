using System.IO;
using System.Reflection;
using System.Web.Mvc;
using log4net;

namespace AcademyPlatform.Web.Umbraco.Controllers
{
    public class PaymentResultController : Controller
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            var stream = Request.InputStream;
            string data = new StreamReader(stream).ReadToEnd();

            var request = new
            {
                data,
                method = Request.HttpMethod,
                Request.Form,
                Request.Headers
            };

            _logger.Info(request);
            return Json(request,JsonRequestBehavior.AllowGet);
        }

    }
}