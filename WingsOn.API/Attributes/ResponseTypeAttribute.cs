using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WingsOn.API.Attributes
{
    public class ResponseTypeAttribute : ProducesResponseTypeAttribute
    {
        public ResponseTypeAttribute(HttpStatusCode statusCode) : base((int)statusCode)
        {
        }

        public ResponseTypeAttribute(Type type, HttpStatusCode statusCode = HttpStatusCode.OK) : base(type, (int)statusCode)
        {
        }

    }
}