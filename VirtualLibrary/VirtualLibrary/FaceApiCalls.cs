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
        private IFaceServiceClient faceServiceClient;

        public FaceApiCalls()
        {
            CreateGroup();
            faceServiceClient = new FaceServiceClient(File.ReadAllText(Application.StartupPath + "/API/APIKEY.txt"), "https://northeurope.api.cognitive.microsoft.com/face/v1.0");
        }

        private async void CreateGroup()
        {
            //await faceServiceClient.CreatePersonGroupAsync(_groupId, _groupName);
        }



        public async void FaceSave(String vardas)
        { 
            CreatePersonResult person = await faceServiceClient.CreatePersonInPersonGroupAsync(_groupId, vardas);
            foreach (string imagePath in Directory.GetFiles(Application.StartupPath + "/" + vardas))
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    await faceServiceClient.AddPersonFaceInPersonGroupAsync(_groupId, person.PersonId, s);
                }
            }
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

