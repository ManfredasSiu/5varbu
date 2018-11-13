using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary
{
    class FaceApiCalls : ICallAzureAPI
    {
        String _groupName = "Users", _groupId = "users";
        private IFaceServiceClient faceServiceClient;

        public FaceApiCalls()
        {
            //CreateGroup();
            faceServiceClient = new FaceServiceClient(ConfigurationManager.AppSettings.Get("apikey"), ConfigurationManager.AppSettings.Get("path"));
        }

        private async void CreateGroup()
        {
            //await faceServiceClient.CreatePersonGroupAsync(_groupId, _groupName);
        }

        public async Task<bool> FaceSave(String vardas) //Veido issaugojimas Resource grupeje
        { 
            CreatePersonResult person = await faceServiceClient.CreatePersonInPersonGroupAsync(_groupId, vardas); //Userio sukurimas
            foreach (string imagePath in Directory.GetFiles(Application.StartupPath + "/" + vardas + "TEMP"))     //Iteruojama visa direktorija
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    try
                    {
                        await faceServiceClient.AddPersonFaceInPersonGroupAsync(_groupId, person.PersonId, s);  //Veido pridejimas
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                        await faceServiceClient.DeletePersonAsync(_groupId, person.PersonId); //Jei issaugoti veido nepavyko - istrinti zmogu is grupes
                        return false;
                    }
                }
            }
            try
            {
                await faceServiceClient.TrainPersonGroupAsync(_groupId); //Grupe istreniruojama su nauju veidu
                return true;
            }
            catch(Exception e)
            {
                await faceServiceClient.DeletePersonAsync(_groupId, person.PersonId); //Jei treniravimas nepavyko - istrinti zmogu is grupes
                return false;
            }
        }

        public async Task<Face[]> UploadAndDetetFaces(string imageFilePath) //Veido atradimas
        {
            try
            {
                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    var faces = await faceServiceClient.DetectAsync(imageFileStream,
                        true,
                        true,
                        new FaceAttributeType[] {
                    FaceAttributeType.Gender,             //Gaunama lytis
                    FaceAttributeType.Age,                //Metai
                    FaceAttributeType.Emotion             //Veido emocija
                        });
                    return faces.ToArray();               //Grazinami visi nuotraukoje rasti veidai su ju atributais
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new Face[0];
            }
        }

        public async Task<String> RecognitionAsync(String TempImgPath) //Veido atpazinimo uzklausa
        {

            Face[] faces = await UploadAndDetetFaces(TempImgPath); //Nuotraukoje atrandami veidai
            var faceIds = faces.Select(face => face.FaceId).ToArray(); //Veidu identifikaciniai numeriai perkeliami i kintamaji

            foreach (var identifyResult in await faceServiceClient.IdentifyAsync(_groupId, faceIds))
            {
                if (identifyResult.Candidates.Length != 0)
                {
                    var candidateId = identifyResult.Candidates[0].PersonId;  //Gauname visus atrastus veidus ir paimame veida arciausiai kameros
                    var person = await faceServiceClient.GetPersonInPersonGroupAsync(_groupId, candidateId); //Gauname naudotojo informacija pagal jo veida
                    return person.Name;
                    // user identificated: person.name is the associated name
                }
            }
            return null;
        }

    }
}

