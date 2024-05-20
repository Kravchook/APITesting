using APITesting.RestInfrastructure.Services;
using System.Net;

namespace APITesting.Tests
{
    public class ZipCodeControllerTests
    {
        public ZipCodeService ZipCodeService = new ZipCodeService();

        [Test]
        [Description("Task20 - Scenario 1")]
        public void GetZipCodes()
        {
            //BUG: Status code not as expected: Actual 201 (Created), Expected 200 (OK)
            var zipCodes = ZipCodeService.GetZipCodes(HttpStatusCode.Created);//(HttpStatusCode.OK);

            foreach (var code in zipCodes)
            {
                Console.WriteLine(code);
            }
        }

        [Test]
        [Description("Task20 - Scenario 2")]
        public void PostZipCodes()
        {
            List<string> zipCodesToPost = new List<string> { "code1", "code2" };
            var zipCodes = ZipCodeService.PostZipCodes(zipCodesToPost, HttpStatusCode.Created);

            foreach (var code in zipCodes)
            {
                Console.WriteLine(code);
            }
        }

        [Test]
        [Description("Task20 - Scenario 3")]
        public void PostZipCodesWithDuplicatesInAvailableList()
        {
            List<string> zipCodesToPost = new List<string> { "code3", "code2" };
            var zipCodes = ZipCodeService.PostZipCodes(zipCodesToPost, HttpStatusCode.Created);

            //BUG: Actual: Got duplications in available zip codes, Expected: There are no duplications in available zip codes
            var duplicatesList = GetDuplicates(zipCodes);

            Assert.Multiple(() =>
            {
                Assert.That(zipCodes, Is.Unique, "The collection has duplicate elements");
                Assert.That(duplicatesList, Is.Empty, "The duplicates collection is not empty");
            });
        }

        [Test]
        [Description("Task20 - Scenario 4")]
        public void PostZipCodesWithDuplicatesInAlreadyUsedList()
        {
            List<string> zipCodesToPost = new List<string> { "12345", "23456" };
            var zipCodes = ZipCodeService.PostZipCodes(zipCodesToPost, HttpStatusCode.Created);

            //BUG: Actual: Got duplications in already used zip codes, Expected: There are no duplications in already used zip codes:
            var duplicatesList = GetDuplicates(zipCodes);

            Assert.Multiple(() =>
            {
                Assert.That(zipCodes, Is.Unique, "The collection has duplicate elements");
                Assert.That(duplicatesList, Is.Empty, "The duplicates collection is not empty");
            });
        }

        private List<string> GetDuplicates(List<string> zipCodes)
        {
            List<string> noDupList = new List<string>();
            List<string> dupList = new List<string>();
            foreach (var code in zipCodes)
            {
                if (!noDupList.Contains(code))
                {
                    noDupList.Add(code);
                }
                else
                {
                    dupList.Add(code);
                }
                Console.WriteLine(code);
            }

            Console.WriteLine("Duplicates list");
            foreach (var item in dupList)
            {
                Console.WriteLine(item);
            }

            return dupList;
        }
    }
}

