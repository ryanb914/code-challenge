using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers {
  public class FlickrRestRequest : IRequest {
    public IResponse Get(string url) {
      if (string.IsNullOrEmpty(url)) {
        return new FlickrFeedResponse(
          new ArgumentNullException("FlickrRestRequest URL cannot be null. Check Web.config."),
          HttpStatusCode.BadRequest
        );
      }

      try {
        // Create web request.
        var request = (HttpWebRequest)WebRequest.Create(url);
        var timeoutMil = 60000;
        request.Timeout = timeoutMil;
        request.ReadWriteTimeout = timeoutMil;
        request.Method = "GET";

        // Get response.
        XElement xmlResponse = null;
        using (var response = request.GetResponse() as HttpWebResponse) {
          if (response == null) {
            return new FlickrFeedResponse(
              new ArgumentNullException("FlickrRestRequest HttpWebResponse.GetResponse returned null."),
              HttpStatusCode.NoContent
            );
          }

          if (response.StatusCode == HttpStatusCode.OK) {
            using (var stm = response.GetResponseStream()) {
              using (var rd = XmlReader.Create(stm)) {
                xmlResponse = XElement.Load(rd);
                return new FlickrFeedResponse() {
                  Xml = xmlResponse
                };
              }
            }
          }
          return new FlickrFeedResponse(
            new WebException($"FlickrRestRequest Http web exception."),
            response.StatusCode,
            response.StatusDescription
          );
        }
      }
      catch (Exception ex) {
        return new FlickrFeedResponse(ex, HttpStatusCode.ExpectationFailed, ex.ToString());
      }
    }

    // Format the XML response from Flickr's API into a FlickrImages object.
    public IResponse FormatXmlToJson(string query, XElement xml) {
      if (xml == null || xml.IsEmpty) {
        return new FlickrImages(query,
          new ArgumentNullException("Cannot format null or empty Flickr XML."),
          HttpStatusCode.NoContent
        );
      }

      var images = new List<FlickrData>();
      XNamespace atom = "http://www.w3.org/2005/Atom";
      foreach (var entry in xml?.Elements(atom + Auxiliary.ENTRY)) {
        string title = entry?.Element(atom + Auxiliary.TITLE)?.Value;
        string published = entry?.Element(atom + Auxiliary.PUBLISHED)?.Value;
        var links = entry?.Elements(atom + Auxiliary.LINK);
        var imageElements = links?.Where(x => x.Attribute(Auxiliary.REL).Value.Equals(Auxiliary.ENCLOSURE));
        string image = imageElements.FirstOrDefault()?.Attribute(Auxiliary.HREF).Value;

        if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(published) && !string.IsNullOrEmpty(image)) {
          images.Add(
            new FlickrData() {
              Title = title,
              PublishedDate = published,
              Image = image
            }
          );
        }
      }

      return new FlickrImages(query) {
        Images = images
      };
    }
  }
}