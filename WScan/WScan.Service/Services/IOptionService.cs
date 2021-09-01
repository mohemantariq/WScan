using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WScan.Shared;

namespace WScan.Service.Services
{
    public interface IOptionService
    {
        public Task SetOption(string Id, string Value);
        public Task<string> GetOptionValue(string Id);
    }
}
