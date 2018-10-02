using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary
{
    class FaceApiCalls
    {
        string _imagePath;
        String _groupName = "Users", _groupId = "users";
        private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("6378d002ef5d4bfa9a479fd767147a00", "https://northeurope.api.cognitive.microsoft.com/face/v1.0");

        public FaceApiCalls()
        {
            CreateGroup();
        }

        private async void CreateGroup()
        {
            await faceServiceClient.CreatePersonGroupAsync(_groupId, _groupName);
        }



        public async void FaceSave(String vardas)
        {
            Face[] faces = await UploadAndDetetFaces(Application.StartupPath + "/Face1.jpg");
            Bitmap img = new Bitmap(Image.FromFile(Application.StartupPath + "/Face1.jpg"));
            CreatePersonResult person = await faceServiceClient.CreatePersonInPersonGroupAsync(_groupId, vardas);
            foreach (string imagePath in Directory.GetFiles(Application.StartupPath + "/" + vardas))
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    await faceServiceClient.AddPersonFaceInPersonGroupAsync(_groupId, person.PersonId, s);
                }
            }
            Directory.CreateDirectory(Application.StartupPath + "/" + vardas);
            img.Save(Application.StartupPath + "/" + vardas + "/face1.jpg");
        }

        private async Task<Face[]> UploadAndDetetFaces(string imageFilePath)
        {
            try
            {
                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    var faces = await faceServiceClient.DetectAsync(imageFileStream,
                        true,
                        true,
                        new FaceAttributeType[] {
                    FaceAttributeType.Gender,
                    FaceAttributeType.Age,
                    FaceAttributeType.Emotion
                        });
                    return faces.ToArray();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new Face[0];
            }
        }

    }
}

