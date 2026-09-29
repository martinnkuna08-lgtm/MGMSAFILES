using Dapper;
using Mandiluve.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Mandiluve.Controllers
{
    public class SurgeonController : Controller
    {
        private IDbConnection _context;

        public SurgeonController(IDbConnection context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
		public IActionResult Admission()
		{
			var user = _context.Query<Admission>
				 ("sp_GetAllAdmission", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user, "AdmissionID", "PatientName");
			return View(user);

		}
		public IActionResult ViewPatient()
		{
			var user = _context.Query<Patient>
				 ("sp_GetAllPatient", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user, "PatientID", "FullName");
			return View(user);
		}
		public IActionResult Create()
		{
			try
			{
				var user = _context.Query<City>
				 ("sp_GetAllCity", null, commandType: CommandType.StoredProcedure);


				ViewData["CityID"] = new SelectList(user, "CityID", "CityName");
				var user1 = _context.Query<Suburb>
				 ("sp_GetAllSuburb", null, commandType: CommandType.StoredProcedure);


				ViewData["SuburbID"] = new SelectList(user1, "SuburbID", "SuburbName");
				var user2 = _context.Query<Province>
				("sp_GetAllProvince", null, commandType: CommandType.StoredProcedure);


				ViewData["ProvinceID"] = new SelectList(user2, "ProvinceID", "ProvinceName");
				return View();
			}
			catch (Exception ex)
			{

				TempData["ErrorMessage"] = "Error: " + ex.Message;
			}
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Patient user)
		{
			try
			{
				DynamicParameters parameters = new();
				parameters.Add("@Title", user.Title, DbType.String);
				parameters.Add("@Name", user.Name, DbType.String);
				parameters.Add("@Surname", user.Surname, DbType.String);


				parameters.Add("@ContactNumber", user.ContactNumber, DbType.String);
				parameters.Add("@Email", user.Email, DbType.String);
				parameters.Add("@DateOfBirth", user.DateOfBirth, DbType.String);
				parameters.Add("@Gender", user.Gender, DbType.String);

				parameters.Add("@AddressLine1", user.AddressLine1, DbType.String);
				parameters.Add("@CityID", user.CityID, DbType.String);
				parameters.Add("@ProvinceID", user.ProvinceID, DbType.String);
				parameters.Add("@SuburbID", user.SuburbID, DbType.String);
				parameters.Add("@Status", user.Status, DbType.String);
				_context.Execute("sp_InsertPatient", parameters, commandType: CommandType.StoredProcedure);
				ModelState.Clear();
				TempData["SuccessMessage"] = "Patient Has Successfully Registered!";
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Error: " + ex.Message;
			}
			var user0 = _context.Query<City>
				("sp_GetAllCity", null, commandType: CommandType.StoredProcedure);


			ViewData["CityID"] = new SelectList(user0, "CityID", "CityName");
			var user1 = _context.Query<Suburb>
			 ("sp_GetAllSuburb", null, commandType: CommandType.StoredProcedure);


			ViewData["SuburbID"] = new SelectList(user1, "SuburbID", "SuburbName");
			var user2 = _context.Query<Province>
			("sp_GetAllProvince", null, commandType: CommandType.StoredProcedure);


			ViewData["ProvinceID"] = new SelectList(user2, "ProvinceID", "ProvinceName");
			return RedirectToAction("ViewPatient");
		}
		public IActionResult UpdateCreate(int PatientID)
		{
			try
			{
				

				var parameters = new DynamicParameters();
				parameters.Add("PatientID", PatientID);
				var user = _context.Query<Patient>("sp_GetPatientByID", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

				var user0 = _context.Query<City>
				 ("sp_GetAllCity", null, commandType: CommandType.StoredProcedure);


				ViewData["CityID"] = new SelectList(user0, "CityID", "CityName");
				var user1 = _context.Query<Suburb>
				 ("sp_GetAllSuburb", null, commandType: CommandType.StoredProcedure);


				ViewData["SuburbID"] = new SelectList(user1, "SuburbID", "SuburbName");
				var user2 = _context.Query<Province>
				("sp_GetAllProvince", null, commandType: CommandType.StoredProcedure);


				ViewData["ProvinceID"] = new SelectList(user2, "ProvinceID", "ProvinceName");
				return View(user);
			}
			catch (Exception ex)
			{

				TempData["ErrorMessage"] = "Error: " + ex.Message;
			}
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult UpdateCreate(Patient user)
		{
			try
			{
				DynamicParameters parameters = new();
				parameters.Add("@Title", user.Title, DbType.String);
				parameters.Add("@Name", user.Name, DbType.String);
				parameters.Add("@Surname", user.Surname, DbType.String);


				parameters.Add("@ContactNumber", user.ContactNumber, DbType.String);
				parameters.Add("@Email", user.Email, DbType.String);
				parameters.Add("@DateOfBirth", user.DateOfBirth, DbType.String);
				parameters.Add("@Gender", user.Gender, DbType.String);

				parameters.Add("@AddressLine1", user.AddressLine1, DbType.String);
				parameters.Add("@CityID", user.CityID, DbType.String);
				parameters.Add("@ProvinceID", user.ProvinceID, DbType.String);
				parameters.Add("@SuburbID", user.SuburbID, DbType.String);
				parameters.Add("@Status", user.Status, DbType.String);
				parameters.Add("@PatientID", user.PatientID, DbType.String);
				_context.Execute("sp_EditPatient", parameters, commandType: CommandType.StoredProcedure);
				ModelState.Clear();
				TempData["SuccessMessage"] = "Patient Detail Successfully Updated!";
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Error: " + ex.Message;
			}
			var user0 = _context.Query<City>
				("sp_GetAllCity", null, commandType: CommandType.StoredProcedure);


			ViewData["CityID"] = new SelectList(user0, "CityID", "CityName");
			var user1 = _context.Query<Suburb>
			 ("sp_GetAllSuburb", null, commandType: CommandType.StoredProcedure);


			ViewData["SuburbID"] = new SelectList(user1, "SuburbID", "SuburbName");
			var user2 = _context.Query<Province>
			("sp_GetAllProvince", null, commandType: CommandType.StoredProcedure);


			ViewData["ProvinceID"] = new SelectList(user2, "ProvinceID", "ProvinceName");
			return RedirectToAction("ViewPatient");
		}
		public IActionResult DeleteCreate(int PatientID)
		{
			try
			{


				var parameters = new DynamicParameters();
				parameters.Add("PatientID", PatientID);
				var user = _context.Query<Patient>("sp_GetPatientByID", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

				var user0 = _context.Query<City>
				 ("sp_GetAllCity", null, commandType: CommandType.StoredProcedure);


				ViewData["CityID"] = new SelectList(user0, "CityID", "CityName");
				var user1 = _context.Query<Suburb>
				 ("sp_GetAllSuburb", null, commandType: CommandType.StoredProcedure);


				ViewData["SuburbID"] = new SelectList(user1, "SuburbID", "SuburbName");
				var user2 = _context.Query<Province>
				("sp_GetAllProvince", null, commandType: CommandType.StoredProcedure);


				ViewData["ProvinceID"] = new SelectList(user2, "ProvinceID", "ProvinceName");
				return View(user);
			}
			catch (Exception ex)
			{

				TempData["ErrorMessage"] = "Error: " + ex.Message;
			}
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteCreate(Patient user)
		{
			try
			{
				DynamicParameters parameters = new();

				parameters.Add("@Status", user.Status, DbType.String);
				parameters.Add("@PatientID", user.PatientID, DbType.String);
				_context.Execute("sp_DeletePatient", parameters, commandType: CommandType.StoredProcedure);
				ModelState.Clear();
				TempData["SuccessMessage"] = "Patient Detail Successfully Updated!";
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Error: " + ex.Message;
			}
			var user0 = _context.Query<City>
				("sp_GetAllCity", null, commandType: CommandType.StoredProcedure);


			ViewData["CityID"] = new SelectList(user0, "CityID", "CityName");
			var user1 = _context.Query<Suburb>
			 ("sp_GetAllSuburb", null, commandType: CommandType.StoredProcedure);


			ViewData["SuburbID"] = new SelectList(user1, "SuburbID", "SuburbName");
			var user2 = _context.Query<Province>
			("sp_GetAllProvince", null, commandType: CommandType.StoredProcedure);


			ViewData["ProvinceID"] = new SelectList(user2, "ProvinceID", "ProvinceName");
			return RedirectToAction("ViewPatient");
		}
		public IActionResult ViewSurgery()
		{
			var user = _context.Query<Surgery>
				 ("sp_GetAllSurgery", null, commandType: CommandType.StoredProcedure);
			ViewData["SurgeryID"] = new SelectList(user, "SurgeryID", "SurgeonName");
			return View(user);
		}
		public IActionResult AddSurgery()
		{
			try
			{
				var user = _context.Query<Patient>
				 ("sp_GetAllPatients", null, commandType: CommandType.StoredProcedure);
				ViewData["PatientID"] = new SelectList(user, "PatientID", "FullName");
				
				var user0 = _context.Query<User>
				("sp_GetAllSurgeon", null, commandType: CommandType.StoredProcedure);
				ViewData["UserID"] = new SelectList(user0, "UserID", "FirstName");
				
				var user1 = _context.Query<User>
				 ("sp_GetAllAnaesthesiologist", null, commandType: CommandType.StoredProcedure);
				ViewData["AnaesthesiologistID"] = new SelectList(user1, "UserID", "FirstName");
				
				var user2 = _context.Query<Theatre>
				("sp_GetAllTheatre", null, commandType: CommandType.StoredProcedure);
				ViewData["TheatreID"] = new SelectList(user2, "TheatreID", "TheatreName");
				
				var user3 = _context.Query<Treatment>
			   ("sp_GetAllTreatment", null, commandType: CommandType.StoredProcedure);
				ViewData["CodeID"] = new SelectList(user3, "CodeID", "CodeName");
				return View();
			}
			catch (Exception ex)
			{

				TempData["ErrorMessage"] = "Error: " + ex.Message;
			}
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddSurgery(Surgery user)
		{
			try
			{
				DynamicParameters parameters = new();
				parameters.Add("@Date", user.Date, DbType.String);
				parameters.Add("@PatientID", user.PatientID, DbType.String);
				parameters.Add("@TimeSlot", user.TimeSlot, DbType.String);
				parameters.Add("@UserID", user.UserID, DbType.String);
				parameters.Add("@AnaesthesiologistID", user.AnaesthesiologistID, DbType.String);
				parameters.Add("@TheatreID", user.TheatreID, DbType.String);
				parameters.Add("@CodeID", user.CodeID, DbType.String);
				parameters.Add("@Status", user.Status, DbType.String);
				_context.Execute("sp_InsertSurgery", parameters, commandType: CommandType.StoredProcedure);
				ModelState.Clear();
				TempData["SuccessMessage"] = "Patient Surgery Details Successfully Added!";
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Error: " + ex.Message;
			}
			var user4 = _context.Query<Patient>
				  ("sp_GetAllPatients", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user4, "PatientID", "FullName");
			var user0 = _context.Query<User>
			("sp_GetAllSurgeon", null, commandType: CommandType.StoredProcedure);
			ViewData["UserID"] = new SelectList(user0, "UserID", "FirstName");
			var user1 = _context.Query<User>
			 ("sp_GetAllAnaesthesiologist", null, commandType: CommandType.StoredProcedure);
			ViewData["AnaesthesiologistID"] = new SelectList(user1, "UserID", "FirstName");
			
			var user2 = _context.Query<Theatre>
			("sp_GetAllTheatre", null, commandType: CommandType.StoredProcedure);
			ViewData["TheatreID"] = new SelectList(user2, "TheatreID", "TheatreName");
		
			var user3 = _context.Query<Treatment>
		   ("sp_GetAllTreatment", null, commandType: CommandType.StoredProcedure);
			ViewData["CodeID"] = new SelectList(user3, "CodeID", "CodeName");
			return View();

		}
		public IActionResult Discharged()
		{
			var user = _context.Query<Surgery>
				 ("sp_GetAllSurgery", null, commandType: CommandType.StoredProcedure);
			ViewData["SurgeryID"] = new SelectList(user, "SurgeryID", "SurgeonName");
			return View(user);
		}
		public IActionResult Discharge(int SurgeryID)
		{
			try
			{
				var parameters = new DynamicParameters();
				parameters.Add("SurgeryID", SurgeryID);
				var user = _context.Query<Surgery>("sp_GetSurgeryByID", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

				var user00 = _context.Query<Patient>
				  ("sp_GetAllPatients", null, commandType: CommandType.StoredProcedure);
				ViewData["PatientID"] = new SelectList(user00, "PatientID", "FullName");
				var user0 = _context.Query<User>
				("sp_GetAllSurgeon", null, commandType: CommandType.StoredProcedure);
				ViewData["UserID"] = new SelectList(user0, "UserID", "FirstName");
				var user1 = _context.Query<User>
				 ("sp_GetAllAnaesthesiologist", null, commandType: CommandType.StoredProcedure);
				ViewData["AnaesthesiologistID"] = new SelectList(user1, "UserID", "FirstName");
				var user2 = _context.Query<Theatre>
				("sp_GetAllTheatre", null, commandType: CommandType.StoredProcedure);
				ViewData["TheatreID"] = new SelectList(user2, "TheatreID", "TheatreName");
				var user3 = _context.Query<Treatment>
			   ("sp_GetAllTreatment", null, commandType: CommandType.StoredProcedure);
				ViewData["CodeID"] = new SelectList(user3, "CodeID", "CodeName");
				return View(user);
			}
			catch (Exception ex)
			{

				TempData["ErrorMessage"] = "Error: " + ex.Message;
			}
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Discharge(Surgery user)
		{
			try
			{
				DynamicParameters parameters = new();
				parameters.Add("@Status", user.Status, DbType.String);
				parameters.Add("@SurgeryID", user.SurgeryID, DbType.String);
				_context.Execute("sp_DischargeSurgery", parameters, commandType: CommandType.StoredProcedure);
				ModelState.Clear();
				TempData["SuccessMessage"] = "Patient Detail Successfully Updated!";
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Error: " + ex.Message;
			}
			var user00 = _context.Query<Patient>
				   ("sp_GetAllPatients", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user00, "PatientID", "FullName");
			var user0 = _context.Query<User>
			("sp_GetAllSurgeon", null, commandType: CommandType.StoredProcedure);
			ViewData["UserID"] = new SelectList(user0, "UserID", "FirstName");
			var user1 = _context.Query<User>
			 ("sp_GetAllAnaesthesiologist", null, commandType: CommandType.StoredProcedure);
			ViewData["AnaesthesiologistID"] = new SelectList(user1, "UserID", "FirstName");
			var user2 = _context.Query<Theatre>
			("sp_GetAllTheatre", null, commandType: CommandType.StoredProcedure);
			ViewData["TheatreID"] = new SelectList(user2, "TheatreID", "TheatreName");
			var user3 = _context.Query<Treatment>
		   ("sp_GetAllTreatment", null, commandType: CommandType.StoredProcedure);
			ViewData["CodeID"] = new SelectList(user3, "CodeID", "CodeName");
			return RedirectToAction("Discharged");

		}
		public IActionResult ViewUrgentPrescription()
		{
			var user = _context.Query<Prescription>
				 ("sp_GetAllPrescription", null, commandType: CommandType.StoredProcedure);
			ViewData["PrescriptionID"] = new SelectList(user, "PrescriptionID", "MedicationName");
			return View(user);
		}
		public IActionResult AddPrescription()
		{
			try
			{
				var user = _context.Query<Patient>
				 ("sp_GetAllPatients", null, commandType: CommandType.StoredProcedure);
				ViewData["PatientID"] = new SelectList(user, "PatientID", "FullName");
				var user1 = _context.Query<HospitalMedication>
				  ("sp_GetAllHospitalMedication", null, commandType: CommandType.StoredProcedure);
				ViewData["HospitalMedicationID"] = new SelectList(user1, "HospitalMedicationID", "MedicationName");
				var user2 = _context.Query<User>
		   ("sp_GetAllUser", null, commandType: CommandType.StoredProcedure);
				ViewData["UserID"] = new SelectList(user2, "UserID", "FirstName");

				return View();
			}
			catch (Exception ex)
			{

				TempData["ErrorMessage"] = "Error: " + ex.Message;
			}
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddPrescription(Prescription user)
		{
			
				DynamicParameters parameters = new();
				parameters.Add("@UserID", user.UserID, DbType.String);
				parameters.Add("@PatientID", user.PatientID, DbType.String);
				parameters.Add("@PrescriptionDate", user.PrescriptionDate, DbType.String);
				parameters.Add("@PrescriptionTime", user.PrescriptionTime, DbType.String);
				parameters.Add("@HospitalMedicationID", user.HospitalMedicationID, DbType.String);
				parameters.Add("@Qty", user.Qty, DbType.String);
			parameters.Add("@Instructions", user.Instructions, DbType.String);
			parameters.Add("@Status", user.Status, DbType.String);
				_context.Execute("sp_InsertPrescription", parameters, commandType: CommandType.StoredProcedure);
				ModelState.Clear();
				TempData["SuccessMessage"] = "Patient Prescription Details Successfully Added!";
			
			var user0 = _context.Query<Patient>
				 ("sp_GetAllPatients", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user0, "PatientID", "FullName");
			var user1 = _context.Query<HospitalMedication>
			  ("sp_GetAllHospitalMedication", null, commandType: CommandType.StoredProcedure);
			ViewData["HospitalMedicationID"] = new SelectList(user1, "HospitalMedicationID", "MedicationName");
			var user2 = _context.Query<User>
			 ("sp_GetAllUser", null, commandType: CommandType.StoredProcedure);
			ViewData["UserID"] = new SelectList(user2, "UserID", "FirstName");

			return RedirectToAction("ViewUrgentPrescription");

		}
		public IActionResult ViewPatientMedication()
		{
			var user = _context.Query<PatientMedication>
				 ("sp_GetAllPatientMedication", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user, "PatientMedicationID", "FullName");
			return View(user);

		}
		public IActionResult Allergies()
		{
			var user = _context.Query<PatientAllergies>
				 ("sp_GetAllPatientAllergies", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user, "AllergiesID", "FullName");
			return View(user);

		}
		public IActionResult ViewPatientCondition()
		{
			var user = _context.Query<PatientCondition>
				 ("sp_GetAllPatientCondition", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user, "ConditionID", "FullName");
			return View(user);
		}
	}
}
