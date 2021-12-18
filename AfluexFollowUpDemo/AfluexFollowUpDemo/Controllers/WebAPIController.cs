﻿using System;

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AfluexFollowUpDemo;
using System.Web.Http;
using System.Data;
using System.IO;
using System.Web;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static AfluexFollowUpDemo.Models.APIModel;
using System.Web.Script.Serialization;

namespace AfluexFollowUpDemo.Controllers
{
    public class WebAPIController : ApiController
    {


        [Route("Login")]
        [HttpPost]
        public HttpResponseMessage Login(LoginModel model)
        {
            try
            {
                DataSet ds = model.Login();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {

                        return Request.CreateResponse(HttpStatusCode.OK,
                            new
                            {
                                StatusCode = HttpStatusCode.OK,
                                Message = "Login Successful",
                                Data = new
                                {
                                    PK_UserId = ds.Tables[0].Rows[0]["Pk_Id"].ToString(),
                                    LoginId = ds.Tables[0].Rows[0]["LoginId"].ToString(),
                                    Name = ds.Tables[0].Rows[0]["Name"].ToString(),
                                    Password = ds.Tables[0].Rows[0]["Password"].ToString(),
                                    EmailId = ds.Tables[0].Rows[0]["EmailId"].ToString(),
                                    Fk_UserTypeId = ds.Tables[0].Rows[0]["Fk_UserTypeId"].ToString(),
                                    UserName = ds.Tables[0].Rows[0]["UserName"].ToString(),
                                    Admin = ds.Tables[0].Rows[0]["Admin"].ToString(),

                                }
                            });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                          new
                          {
                              StatusCode = HttpStatusCode.InternalServerError,
                              //  Message = "Error: " + ds.Tables[0].Rows[0]["ErrorMessage"].ToString(),
                              Message = "Invalid User ID  and Password",

                          });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                         new
                         {
                             StatusCode = HttpStatusCode.InternalServerError,
                             // Message = "Error: ",
                             Message = "Invalid User ID  and Password",
                         });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    new
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        // Message = "Error: " + ex.Message,
                        Message = "Invalid User ID  and Password",

                    });
            }

        }
        [Route("ProductCategoryList")]
        [HttpPost]
        public HttpResponseMessage ProductCategoryList()
        {
            ProductRequest objreports = new ProductRequest();
            List<CategoryDetails> lst = new List<CategoryDetails>();
            List<SubCategoryDetails> lst1 = new List<SubCategoryDetails>();
            DataSet ds = objreports.ProductCategorylist();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    CategoryDetails obj = new CategoryDetails();
                    obj.Pk_CategoryId = r["Pk_CategoryId"].ToString();
                    obj.CategoryName = r["CategoryName"].ToString();
                    lst.Add(obj);
                }
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    SubCategoryDetails obj = new SubCategoryDetails();
                    obj.Pk_ProductCategoryId = r["Pk_ProductCategoryId"].ToString();
                    obj.ProductCategoryName = r["ProductCategoryName"].ToString();
                    obj.Pk_CategoryId = r["Fk_CategoryId"].ToString();
                    lst1.Add(obj);
                }
                return Request.CreateResponse(HttpStatusCode.OK,
              new
              {
                  StatusCode = HttpStatusCode.OK,
                  Message = "Record Found",
                  lstCategory = lst,
                  lstSubCategory = lst1
              });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
             new
             {
                 StatusCode = HttpStatusCode.InternalServerError,
                 Message = "Record Not Found",
                 lstSubCategory = "Record Not Found"
             });
            }
        }
        [Route("AddProcpect")]
        [HttpPost]
        public HttpResponseMessage AddProcpect(SaveProcpect obj)
        {
            obj.FirstInstructionDate = string.IsNullOrEmpty(obj.FirstInstructionDate) ? null : SaveProcpect.ConvertToSystemDate(obj.FirstInstructionDate, "dd/MM/yyyy");
            obj.FollowupDate = string.IsNullOrEmpty(obj.FollowupDate) ? null : SaveProcpect.ConvertToSystemDate(obj.FollowupDate, "dd/MM/yyyy");
            try
            {
                // obj.AddedBy = Pk_ProcpectId;
                DataSet ds = obj.insertProcpect();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {

                        return Request.CreateResponse(HttpStatusCode.OK,
                          new
                          {
                              StatusCode = HttpStatusCode.OK,
                              Message = "Procpect saved successfully",
                          });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError,
                          new
                          {
                              StatusCode = HttpStatusCode.InternalServerError,
                              Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString()
                          });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,
                        new
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            Message = "Error occurred"
                        });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                   new
                   {
                       StatusCode = HttpStatusCode.InternalServerError,
                       Message = "Error: " + ex.Message,

                   });
            }
        }
        [Route("GetProspecctList")]
        [HttpPost]
        public HttpResponseMessage GetProspecctList(ProspectList model)
        {
            List<ProspectLst> lst1 = new List<ProspectLst>();
            try
            {
                DataSet ds = model.ProspectDetails();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ProspectLst obj = new ProspectLst();
                        obj.Pk_ProcpectId = r["Pk_ProcpectId"].ToString();
                        obj.ContactPerson = r["ContactPerson"].ToString();
                        obj.ContactEmailId = r["ContactEmailId"].ToString();
                        obj.ContactNo = r["ContactNo"].ToString();
                        obj.Fk_IndustryCategoryId = r["Fk_IndustryCategoryId"].ToString();
                        obj.CompanyName = r["CompanyName"].ToString();
                        obj.CompanyContactNo = r["CompanyContactNo"].ToString();
                        obj.Address = r["Address"].ToString();
                        lst1.Add(obj);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK,
                  new
                  {
                      StatusCode = HttpStatusCode.OK,
                      Message = "Record Found",
                      lstSubCategory = lst1
                  });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                 new
                 {
                     StatusCode = HttpStatusCode.InternalServerError,
                     Message = "Record Not Found",

                 });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                               new
                               {
                                   StatusCode = HttpStatusCode.InternalServerError,
                                   Message = "Record Not Found",

                               });
            }
        }



        [Route("GetDashBoardTodayFollowup")]
        [HttpPost]
        public HttpResponseMessage GetDashBoardTodayFollowup(TodayVisit model)
        {
            List<Lead> lst3 = new List<Lead>();
            Lead obj3 = new Lead();
            try
            {
                //model.FromDate = DateTime.Now.ToString("MM/dd/yyyy");
                //model.ToDate = DateTime.Now.ToString("MM/dd/yyyy");
                DataSet ds = model.GetDashBoardTodaylsit();
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    Followlistdate obj = new Followlistdate();
                    obj.CompletedMeeting = ds.Tables[1].Rows[0]["CompletedMeeting"].ToString();
                    obj.ScheduleMeeting = ds.Tables[1].Rows[0]["ScheduleMeeting"].ToString();
                    obj.AssignMeeting = ds.Tables[1].Rows[0]["AssignMeeting"].ToString();
                }
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    #region TodayFollowup
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {

                        Lead obj2 = new Lead();

                        obj2.Fk_ProcpectName = r["ContactPerson"].ToString();
                        obj2.ContactNo = r["ContactNo"].ToString();
                        obj2.FirstInstructionDate = r["FirstInstructionDate"].ToString();
                        obj2.ProductCategoryName = r["ProductCategoryName"].ToString();
                        obj2.Fk_SourceName = r["SourceName"].ToString();
                        obj2.Fk_ExecutiveName = r["Name"].ToString();
                        obj2.Fk_ModeInterActionName = r["InterActionName"].ToString();
                        obj2.FollowupDate = r["FollowupDate"].ToString();
                        obj2.Description = r["Description"].ToString();
                        obj2.schedulemeeting = r["schedulemeeting"].ToString();
                        lst3.Add(obj2);
                    }
                    #endregion TodayFollowup

                    return Request.CreateResponse(HttpStatusCode.OK,
                  new
                  {
                      StatusCode = HttpStatusCode.OK,
                      Message = "Record Found",
                      CompletedMeeting = ds.Tables[1].Rows[0]["CompletedMeeting"].ToString(),
                      ScheduleMeeting = ds.Tables[1].Rows[0]["ScheduleMeeting"].ToString(),
                      AssignMeeting = ds.Tables[1].Rows[0]["AssignMeeting"].ToString(),
                      lstTodayFollowup = lst3


                  });
                }
                
           
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                 new
                 {
                     StatusCode = HttpStatusCode.OK,
                     Message = "No schedule meeting for today",
                     CompletedMeeting = ds.Tables[1].Rows[0]["CompletedMeeting"].ToString(),
                     ScheduleMeeting = ds.Tables[1].Rows[0]["ScheduleMeeting"].ToString(),
                     AssignMeeting = ds.Tables[1].Rows[0]["AssignMeeting"].ToString(),
                     lstTodayFollowup = lst3
                 });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                  new
                  {
                      StatusCode = HttpStatusCode.InternalServerError,
                      Message = "Record Not Found",
                  });
            }
        }


        [Route("AddCuurentLogtion")]
        [HttpPost]
        public HttpResponseMessage AddCuurentLogtion(CurrentLocation obj)
        {

            try
            {
                // obj.AddedBy = Pk_ProcpectId;
                DataSet ds = obj.addCurrentLocation();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {

                        return Request.CreateResponse(HttpStatusCode.OK,
                          new
                          {
                              StatusCode = HttpStatusCode.OK,
                              Message = "Add Location saved successfully",
                          });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                          new
                          {
                              StatusCode = HttpStatusCode.InternalServerError,
                              Message = "Employee already exists"
                          });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            Message = "Error occurred"
                        });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                   new
                   {
                       StatusCode = HttpStatusCode.InternalServerError,
                       Message = "Error: " + ex.Message,

                   });
            }
        }
        [Route("GetUsertype")]
        [HttpPost]
        public HttpResponseMessage GetUsertype()
        {
            usertype obj = new usertype();
            List<usertypelst> lst = new List<usertypelst>();
            try
            {
                DataSet dt = obj.BindUserType();

                if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Tables[0].Rows)
                    {
                        usertypelst obj1 = new usertypelst();
                        obj1.userName = r["userName"].ToString();
                        obj1.Pk_UsertypeID = r["Pk_UsertypeID"].ToString();
                        lst.Add(obj1);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK,
             new
             {
                 StatusCode = HttpStatusCode.OK,
                 Message = "Record Found",
                 usertypelst = lst,

             });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new
                   {
                       StatusCode = HttpStatusCode.InternalServerError,
                       Message = "No Record Found",
                       usertypelst = lst,

                   });

                }

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.OK,
                  new
                  {
                      StatusCode = HttpStatusCode.InternalServerError,
                      Message = "Record Not Found",
                  });
            }
        }

        [Route("EmployeeRegistration")]
        [HttpPost]
        public HttpResponseMessage EmployeeRegistration()
        {
            EmpReg model = new EmpReg();
            try
            {
                Random rn = new Random();
                string Results = string.Empty;
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                model.Fk_UserTypeId = HttpContext.Current.Request.Params["Fk_UserTypeId"];
                model.Name = HttpContext.Current.Request.Params["Name"];
                model.ContactNo = HttpContext.Current.Request.Params["ContactNo"];
                model.EmailId = HttpContext.Current.Request.Params["EmailId"];
                model.ContactNo = HttpContext.Current.Request.Params["ContactNo"];
                model.Address = HttpContext.Current.Request.Params["Address"];
                model.CreatedBy = HttpContext.Current.Request.Params["CreatedBy"];
                var file = HttpContext.Current.Request.Files[0];
                model.ProfilePic = rn.Next(10, 100) + "_photo_" + file.FileName;
                string folderPath = HttpContext.Current.Server.MapPath("~/images/ProfilePicture/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                file.SaveAs(folderPath + model.ProfilePic);
                model.ProfilePic = "/images/ProfilePicture/" + model.ProfilePic;
                DataSet ds = model.SaveEmployeeRegistration();
                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    if (ds != null && ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                              new
                              {
                                  StatusCode = HttpStatusCode.OK,
                                  Message = "Employee registration has been successfully",
                              });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,
                          new
                          {
                              StatusCode = HttpStatusCode.OK,
                              Message = "Employee already exists"
                          });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            Message = "Error occurred"
                        });
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                   new
                   {
                       StatusCode = HttpStatusCode.InternalServerError,
                       Message = "Error: " + ex.Message,

                   });
            }
        }
        //[Route("UpdateUserImage")]
        //[HttpPost]
        //public HttpResponseMessage UpdateUserImage()
        //{
        //    uploadprofile model = new uploadprofile();
        //    try
        //    {
        //        Random rn = new Random();
        //        string Results = string.Empty;
        //        if (!Request.Content.IsMimeMultipartContent())
        //        {
        //            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //        }
        //        model.loginid = HttpContext.Current.Request.Params["loginid"];
        //        var file = HttpContext.Current.Request.Files[0];
        //        model.ProfilePic = rn.Next(10, 100) + "_photo_" + file.FileName;
        //        string folderPath = HttpContext.Current.Server.MapPath("~/images/ProfilePicture/");
        //        if (!Directory.Exists(folderPath))
        //        {
        //            Directory.CreateDirectory(folderPath);
        //        }
        //        file.SaveAs(folderPath + model.ProfilePic);
        //        model.ProfilePic = "/images/ProfilePicture/" + model.ProfilePic;
        //        DataSet ds = model.UpdateProFileimage();
        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK,
        //               new
        //               {
        //                   StatusCode = HttpStatusCode.OK,
        //                   Message = "Profile Pic Uploaded Successfully",
        //               });
        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK,
        //               new
        //               {
        //                   StatusCode = HttpStatusCode.OK,
        //                   Message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString(),
        //               });
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK,
        //               new
        //               {
        //                   StatusCode = HttpStatusCode.InternalServerError,
        //                   Message = "Error Occurred",
        //               });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK,
        //               new
        //               {
        //                   StatusCode = HttpStatusCode.InternalServerError,
        //                   Message = ex.Message,
        //               });
        //    }
        //}
        [Route("GetEmployeeRegistrationList")]
        [HttpPost]
        public HttpResponseMessage GetEmployeeRegistrationList(Empreg_List obj)
        {

            List<emplist> lst = new List<emplist>();
            try
            {
                DataSet dt = obj.EmployeeRegistationList();

                if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Tables[0].Rows)
                    {
                        emplist obj1 = new emplist();
                        obj1.loginid = r["loginid"].ToString();
                        obj1.name = r["name"].ToString();
                        obj1.contactno = r["contactno"].ToString();
                        obj1.emailid = r["emailid"].ToString();
                        obj1.Fk_UserTypeId = r["Fk_UserTypeId"].ToString();
                        obj1.username = r["username"].ToString();
                        obj1.Latitude = r["lat"].ToString();
                        obj1.Longitude = r["long"].ToString();
                        lst.Add(obj1);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK,
             new
             {
                 StatusCode = HttpStatusCode.OK,
                 Message = "Record Found",
                 EmpRegistationList = lst,

             });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                   new
                   {
                       StatusCode = HttpStatusCode.InternalServerError,
                       Message = "No Record Found",
                       EmpRegistationList = lst,

                   });

                }

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.OK,
                  new
                  {
                      StatusCode = HttpStatusCode.InternalServerError,
                      Message = "Record Not Found",
                  });
            }
        }

        [Route("businesschance")]
        [HttpPost]
        public HttpResponseMessage businesschance()
        {
            List<LstBusinessChanse> lst = new List<LstBusinessChanse>();
            try
            {
                businesschancelst obj = new businesschancelst();
                DataSet ds = obj.ListBusinessChance();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        LstBusinessChanse list1 = new LstBusinessChanse();
                        list1.Pk_BusinessChanceId = r["Pk_BusinessChanceId"].ToString();
                        list1.BusinessChanceName = r["ChanceName"].ToString();
                        lst.Add(list1);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new
                        {
                            StatusCode = (HttpStatusCode.OK),
                            Message = "Record Found",
                            businesschancelist = lst
                        });

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new
                        {
                            StatusCode = (HttpStatusCode.InternalServerError),
                            Message = "Record Not Found",
                            // businesschancelist = lst
                        });

                }
            }
            catch (Exception ex)
            {
               
                return Request.CreateResponse(HttpStatusCode.OK,
                              new
                              {
                                  StatusCode = HttpStatusCode.InternalServerError,
                                  Message = "Record Not Found",

                              });


            }

        }


        [Route("StateList")]
        [HttpPost]
        public HttpResponseMessage StateList(StateList obj)
        {
           
            List<StateListlst> lst = new List<StateListlst>();
        
            DataSet ds = obj.GetStateCity();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    StateListlst obj1 = new StateListlst();
                    obj1.State = ds.Tables[0].Rows[0]["StateName"].ToString();
                    obj1.City = ds.Tables[0].Rows[0]["CityName"].ToString();
                   
                    lst.Add(obj1);
                }
                return Request.CreateResponse(HttpStatusCode.OK,
              new
              {
                  StatusCode = HttpStatusCode.OK,
                  Message = "Record Found",
                  StateListlst = lst,
                 
              });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
             new
             {
                 StatusCode = HttpStatusCode.InternalServerError,
                 Message = "Record Not Found",
                 lstSubCategory = "Record Not Found"
             });
            }
        }
    }
}



