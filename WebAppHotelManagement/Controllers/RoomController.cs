using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppHotelManagement.Models;
using WebAppHotelManagement.ViewModel;

namespace WebAppHotelManagement.Controllers
{
    public class RoomController : Controller
    {
        readonly HotelDBEntities objHotelDBEntities;
        public RoomController()
        {
            objHotelDBEntities = new HotelDBEntities();
        }
        // GET: Room
        public ActionResult Index()
        {
            RoomViewModel objRoomViewModel = new RoomViewModel
            {
                ListOfBookingStatus = (from obj in objHotelDBEntities.BookingStatus
                                       select new SelectListItem()
                                       {
                                           Text = obj.BookingStatus,
                                           Value = obj.BookingStatusId.ToString(),

                                       }).ToList(),

                ListOfRoomType = (from obj in objHotelDBEntities.RoomTypes
                                  select new SelectListItem()
                                  {
                                      Text = obj.RoomTypeName,
                                      Value = obj.RoomTypeId.ToString(),

                                  }).ToList()
            };

            return View(objRoomViewModel);
        }

        [HttpPost]
        public ActionResult Index(RoomViewModel objRoomViewModel)
        {
            string ImageUniqueName = Guid.NewGuid().ToString();
            string ActualImageName = ImageUniqueName + Path.GetExtension(objRoomViewModel.Image.FileName);
            objRoomViewModel.Image.SaveAs(Server.MapPath("~/RoomImages/" + ActualImageName));

            Room objRoom = new Room()
            {
                RoomNumber = objRoomViewModel.RoomNumber,
                RoomDescription = objRoomViewModel.RoomDescription,
                RoomPrice = objRoomViewModel.RoomPrice,
                BookingStatusId = objRoomViewModel.BookingStatusId,
                IsActive = true,
                RoomImage = ActualImageName,
                RoomCapacity = objRoomViewModel.RoomCapacity,
                RoomTypeId = objRoomViewModel.RoomTypeId

            };

            objHotelDBEntities.Rooms.Add(objRoom);
            objHotelDBEntities.SaveChanges();

            return Json(new { message = "Room Successfully Added.", success = true }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetAllRooms()
        {
            IEnumerable<RoomDetailsViewModel> listOfRoomDetailsViewModel =
                (from objRoom in objHotelDBEntities.Rooms
                 join objBooking in objHotelDBEntities.BookingStatus on objRoom.BookingStatusId equals objBooking.BookingStatusId
                 join objRoomType in objHotelDBEntities.RoomTypes on objRoom.RoomTypeId equals objRoomType.RoomTypeId
                 select new RoomDetailsViewModel()
                 {
                     RoomNumber = objRoom.RoomNumber,
                     RoomDescription = objRoom.RoomDescription,
                     RoomCapacity = objRoom.RoomCapacity,
                     RoomPrice = objRoom.RoomPrice,
                     BookingStatus = objBooking.BookingStatus,
                     RoomType = objRoomType.RoomTypeName,
                     RoomImage = objRoom.RoomImage,
                     RoomId = objRoom.RoomId

                 }).ToList();
            return PartialView("_RoomDetailsPartial", listOfRoomDetailsViewModel);
        }

        [HttpGet]
        public JsonResult EditRoomDetails(int roomId)
        {
            var result = objHotelDBEntities.Rooms.Single(model => model.RoomId == roomId);
            return Json("", JsonRequestBehavior.AllowGet);
        }

    }
}