using ProductProvider.Data.Interfaces;
using ProductProvider.Data.Services;

namespace ProductProvider.Tests.Data_Tests
{
    public class Data_Tests
    {
        private readonly IFileService _fileService;
        private readonly string filePath = Path.GetTempFileName();
        private readonly string content = "Test text!";
        public Data_Tests()
        {
            _fileService = new FileService(filePath);
        }

        [Fact]
        public void SaveToFile_ShouldReturnSuccess_WhenContentIsSaved()
        {
            // act
            var saveToFileResponse = _fileService.SaveToFile(content);

            // assert
            Assert.True(saveToFileResponse.Success);
            Assert.True(File.Exists(filePath));
            
            var savedContent = File.ReadAllText(filePath);
            Assert.Equal($"{content}\r\n", savedContent); // Eftersom jag spara min jsonfil med write Line så var jag tvungen att lägga till radbrytning manuellt ( \r\n )

            File.Delete(filePath);
        }

        [Fact]
        public void GetFromFile_ShouldReturnSuccess_WhenFetchingFromFile()
        {
            // arrange
            File.WriteAllText(filePath, content);

            // act
            var getFromFileResponse = _fileService.GetFromFile();

            // assert
            Assert.Equal(content, getFromFileResponse.Result);

            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void SaveToFile_ShouldOverWriteFileContent_WhenNewContentArrives()
        {
            // Arrange
            var newContent = "New Content!";

            // Act
            _fileService.SaveToFile(content);
            _fileService.SaveToFile(newContent);

            // Assert
            var savedContent = File.ReadAllText(filePath);
            Assert.Equal($"{ newContent}\r\n", savedContent);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Theory]
        [InlineData("Test conten 1")]
        [InlineData("Test conten 2")]
        [InlineData("Test conten 3")]
        [InlineData("Test conten 4")]
        public void SaveToFile_ShouldHandleDifferentContents(string content)
        {
            // Act
            var response = _fileService.SaveToFile(content);

            // Assert
            Assert.True(response.Success);

            var savedContent = File.ReadAllText(filePath);
            Assert.Equal($"{content}\r\n", savedContent);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
