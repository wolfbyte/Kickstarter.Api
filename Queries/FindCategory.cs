using System;
using System.Linq;
using System.Threading.Tasks;
using Kickstarter.Api.Model;

namespace Kickstarter.Api.Queries
{
    public class FindCategory : IQuery<Category>
    {
        private readonly string _name;

        public FindCategory(string name)
        {
            _name = name;
        }

        public async Task<Category> ApplyTo(IKickstarterSession session)
        {
            var categoryList = await session.Query(new AllCategories()).ConfigureAwait(false);
            return categoryList.Categories.FirstOrDefault(c => String.Equals(c.Name, _name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}