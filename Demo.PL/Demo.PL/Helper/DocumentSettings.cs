using System.IO;

namespace Demo.PL.Helper
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file,string folderName)
        {
            //1-get located folder path "ImagesStore"
            var folderPath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/Files", folderName);

            //2-get file name and make sure that the name is unique

            var fileName=$"{Guid.NewGuid()}-{Path.GetFileName(file.FileName)}";

            //3-get file path = folder_path + file_name
            var filePath=Path.Combine(folderPath,fileName);

            //
            using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);

            

            return fileName;
        }
    }
}
