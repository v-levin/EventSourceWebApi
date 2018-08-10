//using EventSourceWebApi.Contracts;
//using Microsoft.Azure.WebJobs;
//using Moq;
//using System;
//using System.Threading.Tasks;

//namespace EventSourceDurableFunctions.Tests
//{
//    public class Tests
//    {
//        public async Task Run()
//        {
//            var durableOrchestrationContextMock = new Mock<DurableOrchestrationContextBase>();

//            var place = new Place()
//            {
//                Name = "Public room",
//                Description = "Place...",
//                Capacity = 35,
//                Location = "Skopje",
//                DateRegistered = DateTime.Now,
//                City = "Skopje"
//            };

//           var placeId =  durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<int>("CreatePlace", place)).ReturnsAsync(place.Id);

//          var updatePlace =   durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<Place>("UpdatePlace", placeId)).ReturnsAsync(place);

//            durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<int>("DeletePlace", place)).ReturnsAsync(updatePlace.);

//            durableOrchestrationContextMock.Setup(x => x.CallActivityAsync<int>("GetPlace", place)).ReturnsAsync(place.Id);

//        }
//    }
//}
