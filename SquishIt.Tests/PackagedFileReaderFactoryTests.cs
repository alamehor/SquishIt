using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using SquishIt.Framework.Files;
using SquishIt.Framework.Tests;

namespace SquishIt.Tests
{
    [TestFixture]
    public class PackagedFileReaderFactoryTests
    {
        IPackagedFileReaderFactory factory;

        [SetUp]
        public void SetUp()
        {
            factory = new PackagedFileReaderFactory();     
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            PackagerTests.delete_all_the_test_files();
        }


        [Test]
        public void CanConfirmExistenceOfAPackagedBundleFileWithMatchingName()
        {
            var testFile = @"/myfilename_combined.css";
            PackagerTests.write_a_test_file_to_the_file_system(testFile);

            Assert.IsTrue(factory.PackagedFileNameExists(testFile));
        }

        [Test]
        public void CanConfirmExistenceOfAPackagedBundleFileWithMatchingNameThatIncludesAWildCardForTheHash()
        {
            var testFile = @"/myfilename_combined_1234448i484884844848848484484844848.css";
            var packageFileName = @"/myfilename_combined_#.css";

            PackagerTests.write_a_test_file_to_the_file_system(testFile);

            Assert.IsTrue(factory.PackagedFileNameExists(packageFileName));
        }

        [Test]
        public void CanReportNonExistenceOfAPackagedBundleFileWithMatchingName()
        {
            var testFile = @"/myfilename_combined.css";

            PackagerTests.delete_test_file_from_the_file_system_if_it_exists(testFile);

            Assert.IsFalse(factory.PackagedFileNameExists(testFile));
        }
        
        [Test]
        public void CanReportNonExistenceOfAPackagedBundleFileWithMatchingNameThatIncludesAWildCardForTheHash()
        {
            var testFile = @"/myfilename_combined_1234448i484884844848848484484844848.css";
            var packageFileName = @"/myfilename_combined_#.css";

            PackagerTests.delete_test_file_from_the_file_system_if_it_exists(testFile);

            Assert.IsFalse(factory.PackagedFileNameExists(packageFileName));
        }

        [Test]
        public void CanRetrieveAPackagedBundleFileNameWithMatchingNameWhenOneExists()
        {
            var testFile = @"/myfilename_combined.css";

            PackagerTests.write_a_test_file_to_the_file_system(testFile);

            Assert.AreEqual(testFile,factory.PackagedFileName(testFile));
        }

        [Test]
        public void CanRetrieveAPackagedBundleFileNameWithMatchingNameWhenOneExistsThatIncludesAWildCardForTheHash()
        {
            PackagerTests.delete_all_the_test_files();

            var testFile = @"/myfilename_combined_1234448i484884844848848484484844848.css";
            var packageFileName = @"/myfilename_combined_#.css";

            PackagerTests.write_a_test_file_to_the_file_system(testFile);

            Assert.AreEqual(testFile,factory.PackagedFileName(packageFileName));
        }

        [Test]
        public void CanRetrieveAPackagedBundleFileNameWithMatchingNameWhenMoreThanOneExistsThatIncludesAWildCardForTheHash()
        {
            PackagerTests.delete_all_the_test_files();

            var testFile1 = @"/myfilename_combined_1234448i484884844848848484484844848.css";
            var testFile2 = @"/myfilename_combined_1234448i484884844848848484334334444.css";
            var packageFileName = @"/myfilename_combined_#.css";

            PackagerTests.write_a_test_file_to_the_file_system(testFile1);
            PackagerTests.write_a_test_file_to_the_file_system(testFile2);
            var found = factory.PackagedFileName(packageFileName);
            Assert.That(found == testFile1 || found == testFile2);
            
        }

        [Test]
        public void WillSelectTheFileWithTheMostRecentTimeStampWhenRetrievingAPackagedBundleFileNameWithMatchingNameWhenMoreThanOneExistsThatIncludesAWildCardForTheHash()
        {
            PackagerTests.delete_all_the_test_files();

            var testFile1 = @"/myfilename_combined_3_234448i484884844848848484484844848.css";
            var testFile2 = @"/myfilename_combined_2_234448i484884844848848484334334444.css";
            var testFile3 = @"/myfilename_combined_1_234448i4848848yyyyyyyyyy4334334444.css"; //notice that if for some reason these were returned in alphanumeric search order, then 3 would be first...just to avoid a "coincidental" test pass. 

            var packageFileName = @"/myfilename_combined_#.css";

            PackagerTests.write_a_test_file_to_the_file_system(testFile1);
            Thread.Sleep(1001);
            PackagerTests.write_a_test_file_to_the_file_system(testFile2);
            Thread.Sleep(1001);
            PackagerTests.write_a_test_file_to_the_file_system(testFile3);

            Assert.AreEqual(testFile1, factory.PackagedFileName(packageFileName));
            
        }


        [Test,ExpectedException(typeof(FileNotFoundException))]
        public void ExpectAFileNotFoundExceptionIfNoMatchingPackagedFileNameExists()
        {
            PackagerTests.delete_all_the_test_files();

            var testFile = @"/myfilename_combined_3_234448i484884844848848484484844848.css";

            var packageFileName = @"/myfilename2_combined_#.css";

            PackagerTests.write_a_test_file_to_the_file_system(testFile);

            var shouldntExist = factory.PackagedFileName(packageFileName);
        }


    }
}