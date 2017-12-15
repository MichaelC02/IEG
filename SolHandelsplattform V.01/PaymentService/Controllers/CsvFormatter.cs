using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using PaymentService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Controllers
{
    public class CsvFormatter : TextOutputFormatter
    {


        public string ContentType { get; private set; }

        public CsvFormatter()
        {
            ContentType = "text/csv";
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.GetEncoding("utf-8"));
        }


        protected override bool CanWriteType(Type type)
        {
            if (typeof(Product).IsAssignableFrom(type)
                || typeof(IEnumerable<Product>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }

        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;

            Type type = context.Object.GetType();
            Type itemType;

            if (type.GetGenericArguments().Length > 0)
            {
                itemType = type.GetGenericArguments()[0];
            }
            else
            {
                itemType = type.GetElementType();
            }

            StringWriter _stringWriter = new StringWriter();

            if (true)
            {
                _stringWriter.WriteLine(
                    string.Join<string>(
                        ";", itemType.GetProperties().Select(x => x.Name)
                    )
                );
            }


            foreach (var obj in (IEnumerable<object>)context.Object)
            {

                var vals = obj.GetType().GetProperties().Select(
                    pi => new
                    {
                        Value = pi.GetValue(obj, null)
                    }
                );

                string _valueLine = string.Empty;

                foreach (var val in vals)
                {

                    if (val.Value != null)
                    {

                        var _val = val.Value.ToString();

                        if (_val.Contains(","))
                            _val = string.Concat("\"", _val, "\"");
                        if (_val.Contains("\r"))
                            _val = _val.Replace("\r", " ");
                        if (_val.Contains("\n"))
                            _val = _val.Replace("\n", " ");
                        _valueLine = string.Concat(_valueLine, _val, ";");

                    }
                    else
                    {
                        _valueLine = string.Concat(_valueLine, string.Empty, ";");
                    }
                }

                _stringWriter.WriteLine(_valueLine.TrimEnd(";".ToCharArray()));
            }

            var streamWriter = new StreamWriter(response.Body);
            await streamWriter.WriteAsync(_stringWriter.ToString());
            await streamWriter.FlushAsync();
        }
    }
}