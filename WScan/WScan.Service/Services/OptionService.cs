using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WScan.Service.Services
{
    public class OptionService : IOptionService
    {
        private readonly DBContext _context;

        public OptionService(DBContext context)
        {
            _context = context;
        }

        public async Task<string> GetOptionValue(string Id)
        {
            var option = await _context.Options.FirstOrDefaultAsync(x => x.Id == Id);
            if (option != null)
                return option.Value;
            else
                return null;
        }

        public async Task SetOption(string Id, string Value)
        {
            var option = await _context.Options.FirstOrDefaultAsync(x => x.Id == Id);
            if (option != null)
                option.Value = Value;
            else await _context.Options.AddAsync(new Models.Option() { Id = Id, Value = Value });
            await _context.SaveChangesAsync();
        }
    }
}
