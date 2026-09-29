using Dapper;
using Mandiluve.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Mandiluve.Controllers
{
    public class NurseController : Controller
    {
        private IDbConnection _context;

        public NurseController(IDbConnection context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewPatient()
        {
            var user = _context.Query<Patient>
                 ("sp_GetAllPatient", null, commandType: CommandType.StoredProcedure);
            ViewData["PatientID"] = new SelectList(user, "PatientID", "FullName");
            return View(user);
        }
		public IActionResult ViewSurgery()
		{
			var user = _context.Query<Surgery>
				 ("sp_GetAllSurgery", null, commandType: CommandType.StoredProcedure);
			ViewData["SurgeryID"] = new SelectList(user, "SurgeryID", "SurgeonName");
			return View(user);
		}
		public IActionResult UnactivePatient()
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
        public IActionResult Admission()
        {
            var user = _context.Query<Admission>
                 ("sp_GetAllAdmission", null, commandType: CommandType.StoredProcedure);
            ViewData["PatientID"] = new SelectList(user, "AdmissionID", "PatientName");
            return View(user);

        }
        public IActionResult Discharged()
        {
            var user = _context.Query<Admission>
                 ("sp_GetAllAdmission", null, commandType: CommandType.StoredProcedure);
            ViewData["PatientID"] = new SelectList(user, "AdmissionID", "PatientName");
            return View(user);

        }
        public IActionResult AdmitPatient()
        {
            try
            {
                var user = _context.Query<Bed>
                 ("sp_GetAllBed", null, commandType: CommandType.StoredProcedure);
                ViewData["BedID"] = new SelectList(user, "BedID", "BedName");

                var user1 = _context.Query<Surgery>
                 ("sp_GetAllSurgerys", null, commandType: CommandType.StoredProcedure);
                ViewData["PatientID"] = new SelectList(user1, "PatientID", "PatientName");

                var user2 = _context.Query<User>
                ("sp_GetAllUser", null, commandType: CommandType.StoredProcedure);
                ViewData["UserID"] = new SelectList(user2, "UserID", "FirstName");

                var user3 = _context.Query<Ward>
                ("sp_GetAllWard", null, commandType: CommandType.StoredProcedure);
                ViewData["WardID"] = new SelectList(user3, "WardID", "WardName");

                var user4 = _context.Query<Treatment>
                ("sp_GetAllTreatment", null, commandType: CommandType.StoredProcedure);
                ViewData["CodeID"] = new SelectList(user4, "CodeID", "CodeName");

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
        public IActionResult AdmitPatient(Admission user)
        {
            try
            {
                DynamicParameters parameters = new();

                parameters.Add("@PatientID", user.PatientID, DbType.String);
                parameters.Add("@UserID", user.UserID, DbType.String);


                parameters.Add("@WardID", user.WardID, DbType.String);
                parameters.Add("@BedID", user.BedID, DbType.String);
                parameters.Add("@AdmissionDate", user.AdmissionDate, DbType.String);
                parameters.Add("@AdmissionTime", user.AdmissionTime, DbType.String);

                parameters.Add("@CodeID", user.CodeID, DbType.String);
                parameters.Add("@Status", user.Status, DbType.String);

                _context.Execute("sp_InsertAdmission", parameters, commandType: CommandType.StoredProcedure);
                ModelState.Clear();
                TempData["SuccessMessage"] = "Patient was Successfully Admitted!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }
            var user0 = _context.Query<Bed>
                  ("sp_GetAllBed", null, commandType: CommandType.StoredProcedure);
            ViewData["BedID"] = new SelectList(user0, "BedID", "BedName");

			var user1 = _context.Query<Surgery>
				 ("sp_GetAllSurgerys", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user1, "PatientID", "PatientName");


			var user2 = _context.Query<User>
            ("sp_GetAllUser", null, commandType: CommandType.StoredProcedure);
            ViewData["UserID"] = new SelectList(user2, "UserID", "FirstName");

            var user3 = _context.Query<Ward>
            ("sp_GetAllWard", null, commandType: CommandType.StoredProcedure);
            ViewData["WardID"] = new SelectList(user3, "WardID", "WardName");

            var user4 = _context.Query<Treatment>
            ("sp_GetAllTreatment", null, commandType: CommandType.StoredProcedure);
            ViewData["CodeID"] = new SelectList(user4, "CodeID", "CodeName");
			return RedirectToAction("Admission");
		}
        public IActionResult Discharge(int AdmissionID)
        {
            try
            {


                var parameters = new DynamicParameters();
                parameters.Add("AdmissionID", AdmissionID);
                var user = _context.Query<Admission>("sp_GetAdmissionByID", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                var user0 = _context.Query<Bed>
                  ("sp_GetAllBed", null, commandType: CommandType.StoredProcedure);
                ViewData["BedID"] = new SelectList(user0, "BedID", "BedName");

				var user1 = _context.Query<Surgery>
				("sp_GetAllSurgerys", null, commandType: CommandType.StoredProcedure);
				ViewData["PatientID"] = new SelectList(user1, "PatientID", "PatientName");

				var user2 = _context.Query<User>
                ("sp_GetAllUser", null, commandType: CommandType.StoredProcedure);
                ViewData["UserID"] = new SelectList(user2, "UserID", "FirstName");

                var user3 = _context.Query<Ward>
                ("sp_GetAllWard", null, commandType: CommandType.StoredProcedure);
                ViewData["WardID"] = new SelectList(user3, "WardID", "WardName");

                var user4 = _context.Query<Treatment>
                ("sp_GetAllTreatment", null, commandType: CommandType.StoredProcedure);
                ViewData["CodeID"] = new SelectList(user4, "CodeID", "CodeName");

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
        public IActionResult Discharge(Admission user)
        {
            try
            {
                DynamicParameters parameters = new();

                parameters.Add("@Status", user.Status, DbType.String);
                parameters.Add("@AdmissionID", user.AdmissionID, DbType.String);
                _context.Execute("sp_DischargeUser", parameters, commandType: CommandType.StoredProcedure);
                ModelState.Clear();
                TempData["SuccessMessage"] = "Patient Detail Successfully Updated!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }
            var user0 = _context.Query<Bed>
                  ("sp_GetAllBed", null, commandType: CommandType.StoredProcedure);
            ViewData["BedID"] = new SelectList(user0, "BedID", "BedName");

			var user1 = _context.Query<Surgery>
				 ("sp_GetAllSurgerys", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user1, "PatientID", "PatientName");

			var user2 = _context.Query<User>
            ("sp_GetAllUser", null, commandType: CommandType.StoredProcedure);
            ViewData["UserID"] = new SelectList(user2, "UserID", "FirstName");

            var user3 = _context.Query<Ward>
            ("sp_GetAllWard", null, commandType: CommandType.StoredProcedure);
            ViewData["WardID"] = new SelectList(user3, "WardID", "WardName");

            var user4 = _context.Query<Treatment>
            ("sp_GetAllTreatment", null, commandType: CommandType.StoredProcedure);
            ViewData["CodeID"] = new SelectList(user4, "CodeID", "CodeName");
			return RedirectToAction("Admission");

		}
        public IActionResult ViewPatientCondition()
        {
            var user = _context.Query<PatientCondition>
                 ("sp_GetAllPatientCondition", null, commandType: CommandType.StoredProcedure);
            ViewData["PatientID"] = new SelectList(user, "ConditionID", "FullName");
            return View(user);
        }
        public IActionResult PatientCondition()
        {
            try
            {
				var user0 = _context.Query<Surgery>
				("sp_GetAllAdmissionS", null, commandType: CommandType.StoredProcedure);
				ViewData["PatientID"] = new SelectList(user0, "PatientID", "PatientName");

				var user1 = _context.Query<ChronicCondition>
                 ("sp_GetAllChronicCondition", null, commandType: CommandType.StoredProcedure);
                ViewData["ICDID"] = new SelectList(user1, "ICDID", "Diagnosis");
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
        public IActionResult PatientCondition(PatientCondition user)
        {
            try
            {
                DynamicParameters parameters = new();

                parameters.Add("@PatientID", user.PatientID, DbType.String);
                parameters.Add("@ICDID", user.ICDID, DbType.String);

                _context.Execute("sp_InsertPatientCondition", parameters, commandType: CommandType.StoredProcedure);
                ModelState.Clear();
                TempData["SuccessMessage"] = "Patient Chronics Successfully Added!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }
			var user0 = _context.Query<Surgery>
				("sp_GetAllAdmissionS", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user0, "PatientID", "PatientName");

			var user1 = _context.Query<ChronicCondition>
             ("sp_GetAllChronicCondition", null, commandType: CommandType.StoredProcedure);
            ViewData["ICDID"] = new SelectList(user1, "ICDID", "Diagnosis");
			return RedirectToAction("ViewPatientCondition");
		}
        public IActionResult Allergies()
        {
            var user = _context.Query<PatientAllergies>
                 ("sp_GetAllPatientAllergies", null, commandType: CommandType.StoredProcedure);
            ViewData["PatientID"] = new SelectList(user, "AllergiesID", "FullName");
            return View(user);

        }
        public IActionResult ViewPatientMedication()
        {
            var user = _context.Query<PatientMedication>
                 ("sp_GetAllPatientMedication", null, commandType: CommandType.StoredProcedure);
            ViewData["PatientID"] = new SelectList(user, "PatientMedicationID", "FullName");
            return View(user);

        }
        public IActionResult PatientMedication()
        {
            try
            {
				var user0 = _context.Query<Surgery>
				("sp_GetAllAdmissionS", null, commandType: CommandType.StoredProcedure);
				ViewData["PatientID"] = new SelectList(user0, "PatientID", "PatientName");

				var user1 = _context.Query<ChronicMedication>
                 ("sp_GetAllChronicMedication", null, commandType: CommandType.StoredProcedure);
                ViewData["ChronicMedicationID"] = new SelectList(user1, "ChronicMedicationID", "MedicationName");
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
        public IActionResult PatientMedication(PatientMedication user)
        {
            try
            {
                DynamicParameters parameters = new();

                parameters.Add("@PatientID", user.PatientID, DbType.String);
                parameters.Add("@ChronicMedicationID", user.ChronicMedicationID, DbType.String);

                _context.Execute("sp_InsertPatientMedication", parameters, commandType: CommandType.StoredProcedure);
                ModelState.Clear();
                TempData["SuccessMessage"] = "Patient Chronic Medication Successfully Added!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }
			var user0 = _context.Query<Surgery>
				("sp_GetAllAdmissionS", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user0, "PatientID", "PatientName");
			var user1 = _context.Query<ChronicMedication>
             ("sp_GetAllChronicMedication", null, commandType: CommandType.StoredProcedure);
            ViewData["ChronicMedicationID"] = new SelectList(user1, "ChronicMedicationID", "MedicationName");
			return RedirectToAction("ViewPatientMedication");
		}
        public IActionResult PatientAllergies()
        {
            try
            {
				var user0 = _context.Query<Surgery>
				("sp_GetAllAdmissionS", null, commandType: CommandType.StoredProcedure);
				ViewData["PatientID"] = new SelectList(user0, "PatientID", "PatientName");

				var user1 = _context.Query<Ingredient>
                 ("sp_GetAllActiveIngredient", null, commandType: CommandType.StoredProcedure);
                ViewData["IngredientID"] = new SelectList(user1, "IngredientID", "ActiveIngredient");
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
        public IActionResult PatientAllergies(PatientAllergies user)
        {
            try
            {
                DynamicParameters parameters = new();

                parameters.Add("@IngredientID", user.IngredientID, DbType.String);
                parameters.Add("@PatientID", user.PatientID, DbType.String);

                _context.Execute("sp_InsertPatientAllergies", parameters, commandType: CommandType.StoredProcedure);
                ModelState.Clear();
                TempData["SuccessMessage"] = "Patient Chronics Successfully Added!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }
			var user0 = _context.Query<Surgery>
				("sp_GetAllAdmissionS", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user0, "PatientID", "PatientName");

			var user1 = _context.Query<Ingredient>
             ("sp_GetAllActiveIngredient", null, commandType: CommandType.StoredProcedure);
            ViewData["IngredientID"] = new SelectList(user1, "IngredientID", "ActiveIngredient");
			return RedirectToAction("Allergies");
		}
        public IActionResult ViewPatientVital()
        {
            var user = _context.Query<PatientVital>
                 ("sp_GetAllPatientVital", null, commandType: CommandType.StoredProcedure);
            ViewData["PatientID"] = new SelectList(user, "PatientID", "PatientName");
            return View(user);
        }
        public IActionResult AddPatientVital()
        {
            try
            {


				var user0 = _context.Query<Surgery>
				("sp_GetAllAdmissionS", null, commandType: CommandType.StoredProcedure);
				ViewData["PatientID"] = new SelectList(user0, "PatientID", "PatientName");



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
        public IActionResult AddPatientVital(PatientVital user)
        {
            try
            {
                DynamicParameters parameters = new();

                parameters.Add("@PatientID", user.PatientID, DbType.String);
                parameters.Add("@BodyTemperature", user.BodyTemperature, DbType.String);


                parameters.Add("@HeartRate", user.HeartRate, DbType.String);
                parameters.Add("@BloodPressure", user.BloodPressure, DbType.String);
                parameters.Add("@OxygenSaturation", user.OxygenSaturation, DbType.String);
                parameters.Add("@VitalTime", user.VitalTime, DbType.String);

                _context.Execute("sp_InsertPatientVital", parameters, commandType: CommandType.StoredProcedure);
                ModelState.Clear();
                TempData["SuccessMessage"] = "Patient Vital was Successfully Added!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }
			var user0 = _context.Query<Surgery>
				("sp_GetAllAdmissionS", null, commandType: CommandType.StoredProcedure);
			ViewData["PatientID"] = new SelectList(user0, "PatientID", "PatientName");


			return RedirectToAction("ViewPatientVital");
		}
        public IActionResult ViewDispensedPrescription()
        {
            var user = _context.Query<Prescription>
                 ("sp_GetAllPrescription", null, commandType: CommandType.StoredProcedure);
            ViewData["PrescriptionID"] = new SelectList(user, "PrescriptionID", "MedicationName");
            return View(user);
        }
        public IActionResult AdministerMedication(int PrescriptionID)
        {
            try
            {


                var parameters = new DynamicParameters();
                parameters.Add("PrescriptionID", PrescriptionID);
                var user = _context.Query<Prescription>("sp_GetPrescriptionByID", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();



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
        public IActionResult AdministerMedication(Prescription user)
        {
            try
            {
                DynamicParameters parameters = new();

                parameters.Add("@Status", user.Status, DbType.String);
                parameters.Add("@ReceivedDate", user.ReceivedDate, DbType.String);
                parameters.Add("@ReceivedTime", user.ReceivedTime, DbType.String);
                parameters.Add("@PrescriptionID", user.PrescriptionID, DbType.String);
                parameters.Add("@Qty", user.Qty, DbType.String);
                _context.Execute("sp_AdministerMedication", parameters, commandType: CommandType.StoredProcedure);
                ModelState.Clear();
                TempData["SuccessMessage"] = "Patient Medication Successfully Administered!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }

            return View();

        }
        public IActionResult ViewReceivedMedication()
        {
            var user = _context.Query<Prescription>
                 ("sp_GetAllPrescription", null, commandType: CommandType.StoredProcedure);
            ViewData["PrescriptionID"] = new SelectList(user, "PrescriptionID", "MedicationName");
            return View(user);
        }
    }
}
