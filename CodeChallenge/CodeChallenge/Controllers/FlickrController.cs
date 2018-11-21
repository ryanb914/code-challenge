using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers {
  [RoutePrefix("flickr")]
  public class FlickrController : ApiController {
    private IRequest _flickrRestRequest;

    public FlickrController(IRequest flickrRestRequestr) {
      _flickrRestRequest = flickrRestRequestr;
    }

    [HttpGet]
    [Route("images")]
    public FlickrImages GetFlickrFeed([FromUri]string q) {
      if (string.IsNullOrEmpty(q)) {
        return new FlickrImages(q,
          new ArgumentNullException("Parameter \"q\" value cannot be null or empty."),
          HttpStatusCode.BadRequest
        );
      }

      // Call Flickr's REST API.
      var baseUrl = ConfigurationManager.AppSettings["FlickrBaseUrl"];
      var url = $"{baseUrl}?tags={q}";
      IResponse response = _flickrRestRequest.Get(url);

      // Error Handling.
      if (response == null) {
        return new FlickrImages(q,
          new ArgumentNullException("FlickrRestRequest.Get response cannot be null."),
          HttpStatusCode.NoContent
        );
      }
      else if (response.IsError) {
        return new FlickrImages(q,
          response.Ex,
          response.StatusCode,
          response.StatusMessage
        );
      }

      // Return formated JSON object.
      return (FlickrImages)_flickrRestRequest.FormatXmlToJson(q, response.Xml);
    }
  }
}
