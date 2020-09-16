using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Net;

namespace catalogs.functionaltests
{
    public class CatalogScenario : CatalogScenarioBase
    {
        [Fact]
        public async Task Get_get_all_catalogitems_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync(Get.Items());

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Get_get_catalogitem_by_id_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync(Get.ItemById(1));

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Get_get_catalogitem_by_id_and_response_bad_request_status_code()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync(Get.ItemById(int.MinValue));

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }
        [Fact]
        public async Task Get_get_catalogitem_by_id_and_response_not_found_status_code()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync(Get.ItemById(int.MaxValue));

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}
