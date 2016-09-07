namespace AcademyPlatform.Web.Infrastructure.Extensions
{
    using System;
    using System.Web;

    public static class HttpRequestExtensions
    {
       	public static string GetOriginalUrlWithDomain(this HttpRequestBase request)
		{
			return request.Url.GetLeftPart(UriPartial.Authority) + request.RawUrl;
		}

        public static string GetHttpOriginalUrl(this HttpRequestBase request)
		{
			string url = request.GetOriginalUrlWithDomain();
			if (request.Url.Scheme == Uri.UriSchemeHttp)
			{
				return url;
			}
			return Uri.UriSchemeHttp + url.Substring(request.Url.Scheme.Length);
		}
    }
}
