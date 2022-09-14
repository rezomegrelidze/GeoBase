using GeoBase.API.DataLayer;
using GeoBase.API.Tests.Infrastructure;
using Xunit.Abstractions;

namespace GeoBase.API.Tests
{

    public class GeoDatabaseTests
    {
        // Tests that were mostly used to experiment with the code

        private readonly ITestOutputHelper _helper;
        private readonly Database database;

        public GeoDatabaseTests(ITestOutputHelper helper)
        {
            _helper = helper;
            database = new Database();
        }

        [Fact]
        public void ShouldGetVersion()
        {
            Assert.True(database.Header.Version != 0);
            _helper.WriteLine(database.Header.Version.ToString());
            _helper.WriteLine(database.Header.DatabaseName);
        }

        [Fact]
        public void ShouldGetIntervalsLocationsAndIndexes()
        {
            var ranges = database.Ranges;

            foreach (var interval in ranges.Take(10))
            {
                _helper.WriteLine(interval.RenderMembers());
            }

            var locations = database.Locations;
            

            foreach (var location in locations.Take(10))
            {
                _helper.WriteLine(location.RenderMembers());
            }

            var cities = database.Cities;

            foreach (var index in cities.Take(10))
            {
                _helper.WriteLine(index.ToString());
            }
        }
    }
}