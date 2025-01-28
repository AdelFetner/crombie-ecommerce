using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crombie_ecommerce.BusinessLogic
{
    public class HistoryService<T> where T : class, IProcessableEntity
    {
        private readonly ShopContext _context;
        public HistoryService(ShopContext context)
        {
            _context = context;
        }
        public async Task<bool> ProcessEntityAsync<THistory>(
            Guid entityId,
            string processedBy = "Unregisted"
        ) where THistory : History, new()
        {
            var entity = await _context.Set<T>().FindAsync(entityId);
            if (entity == null)
                return false;
            var historyEntity = new THistory
            {
                OriginalId = entity.Id,
                UserId = entity.UserId,
                ProcessedAt = DateTime.UtcNow,
                ProcessedBy = processedBy,
                EntityJson = entity.SerializeToJson()
            };
            _context.Set<THistory>().Add(historyEntity);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}   
