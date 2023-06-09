using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SecureGroup.DBContexts;
using SecureGroup.ViewModel;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

namespace SecureGroup.Models
{
    public class DataAccessLayerLinq 
    {
        private MsDBContext myDbContext;

        public DataAccessLayerLinq(MsDBContext context) 
        {
            myDbContext = context;
        }

        //public DataAccessLayerLinq(DbContextOptions<MsDBContext> options, MsDBContext context) : base(options)
        //{
        //    myDbContext = context;
        //}

        public List<WarehouseViewModel> GetWarehouseList(int Id)
        {
            List<WarehouseViewModel> warehouseViewModel = new List<WarehouseViewModel>();

            //warehouseViewModel = (from Warehouse in myDbContext.WareHouse.Cast<WarehouseViewModel>()
            //                      select Warehouse).ToList();

            //var a =  (from city in myDbContext.CityMaster.Cast<CityMaster>()
            //           select city).ToList();

            warehouseViewModel = (from wh in myDbContext.WareHouse.Cast<WareHouse>()
                     join ad in myDbContext.WareHouseAdminDetails on wh.WareHouseAdminId equals ad.WareHouseAdminId
                     join c in myDbContext.CountryMaster on wh.CountryId equals c.ID
                     join s in myDbContext.StateMaster on wh.StateId equals s.ID
                     join ct in myDbContext.CityMaster on wh.CityId equals ct.ID
                     orderby wh.WarehouseId
                     select new WarehouseViewModel
                     {
                         WarehouseId=wh.WarehouseId,
                         WarehouseName=wh.WarehouseName,
                         Address=wh.Address,
                         CityId = wh.CityId,
                         City = ct.Name,
                         StateId = wh.StateId,
                         State = s.Name,
                         ZipCode = wh.ZipCode,
                         CountryId=wh.CountryId,
                         Country = c.Name,
                         AdminName = ad.UserName,
                         AdminContactNo = ad.ContactNumber,
                         AdminEmailId = ad.Email,
                     }).ToList();

           

            return warehouseViewModel;
        }

        public List<SelectListItem> GetDropDownListData(string DDName,int? Id)
        {

            var list = new List<SelectListItem>();
            list.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = "0"
            });

            if (DDName == "Country")
            {
                list = (from country in myDbContext.CountryMaster
                                   select new SelectListItem()
                                   {
                                       Text = country.Name,
                                       Value = country.ID.ToString(),
                                   }).ToList();
            }

            if (DDName == "City")
            {
                list = (from city in myDbContext.CityMaster where city.StateID == Id
                        select new SelectListItem()
                        {
                            Text = city.Name,
                            Value = city.ID.ToString(),
                        }).ToList();
            }

            if (DDName == "State")
            {
                list = (from state in myDbContext.StateMaster where state.CountryID == Id
                        select new SelectListItem()
                        {
                            Text = state.Name,
                            Value = state.ID.ToString(),
                        }).ToList();
            }

            if (DDName == "User")
            {
                list = (from user in myDbContext.User
                        //where user.RoleId == Id
                        select new SelectListItem()
                        {
                            Text = user.Name,
                            Value = user.UserId.ToString(),
                        }).ToList();
            }


            return list;

        }


        public UserDetailsViewModel GetUserDetails(int Id)
        {
            UserDetailsViewModel userDetails = new UserDetailsViewModel();

            //warehouseViewModel = (from Warehouse in myDbContext.WareHouse.Cast<WarehouseViewModel>()
            //                      select Warehouse).ToList();

            //var a =  (from city in myDbContext.CityMaster.Cast<CityMaster>()
            //           select city).ToList();

            userDetails = (from wh in myDbContext.User.Cast<UserViewModel>()
                                  join ad in myDbContext.WareHouseAdminDetails on wh.UserId equals ad.UserId
                                  
                                  select new UserDetailsViewModel
                                  {
                                      UserId = wh.UserId,
                                      Name = wh.Name,
                                      Email = wh.Email,
                                      ContactNo = ad.ContactNumber,                                     
                                  }).FirstOrDefault();

            return userDetails;
        }
    }
}
