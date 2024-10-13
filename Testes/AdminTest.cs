using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using API.Shared;
using Xunit.Abstractions;

namespace Testes
{ 
    public class AdminTest: IDisposable
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper output;
        //_

        private async Task<string> GetJWT()
        {
            // passando os dados de login
            object payload = new  {
                email = "fulanodetal@gmail.com",
                password = "123456"
            };

            using StringContent jsonContent = new (
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            using HttpResponseMessage response = await _client.PostAsync(
            "admin/login",
            jsonContent);
            
            response.EnsureSuccessStatusCode();

            string jwt = await response.Content.ReadAsStringAsync(); // obtendo o token

            return jwt.Replace("\"", "");
        }

        public AdminTest(ITestOutputHelper output)
        {
            _client = new HttpClient() {BaseAddress = new Uri("http://localhost:5229")};
            this.output = output;
            string jwt = GetJWT().Result; // pega sem await
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}"); // token de autenticação
        }

        [Fact]
        public async Task Test_Create_Admin_Must_Work()
        {
            // criando um novo registro com campos válidos
            object payload = new {
                email = "cicranodasilva@hotmail.com",
                password = "senhamuitoforte123",
                username = "oCricrano"
            }; 

            using StringContent jsonContent = new (
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            using HttpResponseMessage response = await _client.PostAsync(
            "admin",
            jsonContent);

            response.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            string result = await response.Content.ReadAsStringAsync();
            int userId;

            Assert.True(int.TryParse(result, out userId));
            Assert.True(userId > 0);
        }
        
        [Fact]
        public async Task Test_Create_Admin_Shouldnt_Work()
        {
            // tentando criar um novo registro com alguns campos inválidos
            object payload = new {
                email = "cicranodasilva@hotmail.com",
                password = "",
                username = "oCricrano"
            };

            using StringContent jsonContent = new (
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            using HttpResponseMessage response = await _client.PostAsync(
            "admin",
            jsonContent);

            Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Fact]
        public async Task Test_Update_Admin_Must_Work()
        {
            // criando um novo registro para o update
            object payload = new {
                email = "beltranobraga@hotmail.com",
                password = "senhamuitomaisforte123",
                username = "Beltrano"
            };

            using StringContent jsonContent = new (
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            using HttpResponseMessage response = await _client.PostAsync(
            "admin",
            jsonContent);

            response.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            string result = await response.Content.ReadAsStringAsync();
            int userId;

            Assert.True(int.TryParse(result, out userId));
            Assert.True(userId > 0);

            // testando o update

            object payloadForUpdate = new {
                email = "beltranob@hotmail.com",
                password = "essaéasenhamaisfortequepus",
                username = "SóBeltrano"
            };

            using StringContent jsonContentForUpdate = new (
                JsonSerializer.Serialize(payloadForUpdate),
                Encoding.UTF8,
                "application/json"
            );

            using HttpResponseMessage responseForUpdate = await _client.PutAsync(
            $"admin/{userId}",
            jsonContentForUpdate);

            responseForUpdate.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, responseForUpdate.StatusCode);

            string resultForUpdate = await responseForUpdate.Content.ReadAsStringAsync();

            AdminDto? admin = JsonSerializer.Deserialize<AdminDto>(resultForUpdate);
            
            Assert.NotNull(admin);

            //vendo se os campos se correspondem
            Assert.Equal("beltranob@hotmail.com", admin.Email);
            Assert.Equal("essaéasenhamaisfortequepus", admin.Password);
            Assert.Equal("SóBeltrano", admin.UserName);
        }

        [Fact]
        public async Task Test_Update_Admin_Shouldnt_Work()
        {
            // criando um novo registro para o update
            object payload = new {
                email = "estousemmaisnomesengracados@hotmail.com",
                password = "qualquercoisaserviaaqui",
                username = "engraçado"
            };

            using StringContent jsonContent = new (
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            using HttpResponseMessage response = await _client.PostAsync(
            "admin",
            jsonContent);

            response.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            string result = await response.Content.ReadAsStringAsync();
            int userId;

            Assert.True(int.TryParse(result, out userId));
            Assert.True(userId > 0);

            // tentando alterar o registro com alguns campos inválidos
            object payloadForUpdate = new {
                email = "beltranob@hotmail.com",
                password = "", // não pode isso
                username = "VirouBeltrano"
            };

            using StringContent jsonContentForUpdate = new (
                JsonSerializer.Serialize(payloadForUpdate),
                Encoding.UTF8,
                "application/json"
            );

            using HttpResponseMessage responseForUpdate = await _client.PutAsync(
            $"admin/{userId}",
            jsonContentForUpdate);

            Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, responseForUpdate.StatusCode); // tem campo inválido
        }

        [Fact]
        public async Task Test_Get_Admin_Must_Work()
        {
            // criando um registro para obtê-lo depois
            object payload = new {
                email = "dandomaisideias@outlook.com",
                password = "jsjsjsjsjjsjsjsaleatorio",
                username = "serve"
            };

            using StringContent jsonContent = new (
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            using HttpResponseMessage response = await _client.PostAsync(
            "admin",
            jsonContent);

            response.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            string result = await response.Content.ReadAsStringAsync();
            int userId;

            Assert.True(int.TryParse(result, out userId));
            Assert.True(userId > 0);

            // tentando o obter o registro recém criado
            using HttpResponseMessage responseForGet = await _client.GetAsync($"admin/{userId}");

            responseForGet.EnsureSuccessStatusCode();

            string resultGet = await responseForGet.Content.ReadAsStringAsync();

            AdminDto? admin = JsonSerializer.Deserialize<AdminDto>(resultGet);

            Assert.NotNull(admin);

            // vendo se os campos se correspondem
            Assert.Equal("dandomaisideias@outlook.com", admin.Email);
            Assert.Equal("jsjsjsjsjjsjsjsaleatorio", admin.Password);
            Assert.Equal("serve", admin.UserName);
        }

        [Fact]
        public async Task Test_Get_All_Admin_Must_Work()
        {
            // testando obter todos os registros
            using HttpResponseMessage response = await _client.GetAsync("admin/all");

            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            string result = await response.Content.ReadAsStringAsync();

            List<AdminDto>? admins = JsonSerializer.Deserialize<List<AdminDto>>(result);

            Assert.NotNull(admins);
        }

        [Fact]
        public async Task Test_Delete_Admin_Must_Work()
        {
            // criando um registro para deletá-lo depois
            object payload = new {
                email = "sótestandoodelete@hotmail.com",
                password = "dkdkkdkdkdkdkdkkdkd",
                username = "ddmdmm"
            };

            using StringContent jsonContent = new (
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            using HttpResponseMessage response = await _client.PostAsync(
            "admin",
            jsonContent);

            response.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            string result = await response.Content.ReadAsStringAsync();
            int userId;

            Assert.True(int.TryParse(result, out userId));

            // tentando deletar o registro
            using HttpResponseMessage responseDelete = await _client.DeleteAsync($"admin/{userId}");

            responseDelete.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.NoContent, responseDelete.StatusCode);
        }

        [Fact]
        public async Task Test_Delete_Admin_Shouldnt_Work()
        { 
            // tentando deletar um registro inexistente
            using HttpResponseMessage response = await _client.DeleteAsync("admin/-1");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode); // notfound é o retorno
        }
        
        public void Dispose()
        {
            _client.Dispose();
        }
    }
}