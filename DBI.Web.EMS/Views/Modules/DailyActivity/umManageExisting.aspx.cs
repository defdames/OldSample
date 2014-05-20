using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Data.Objects;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using DBI.Core.Security;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;


namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
	public partial class umManageExisting : BasePage
	{

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
			{
				X.Redirect("~/Views/uxDefault.aspx");

			}
			if (!X.IsAjaxRequest && !IsPostBack)
			{
				this.uxRedWarning.Value = ResourceManager.GetInstance().GetIconUrl(Icon.Exclamation);
				this.uxYellowWarning.Value = ResourceManager.GetInstance().GetIconUrl(Icon.Error);
			}
		}

		protected long GetOrgId(long HeaderId)
		{
			using (Entities _context = new Entities())
			{
				long OrgId;
				return OrgId = (from d in _context.DAILY_ACTIVITY_HEADER
								join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
								where d.HEADER_ID == HeaderId
								select (long)p.ORG_ID).Single();
			}

		}

		/// <summary>
		/// Gets filterable list of header data
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void deReadHeaderData(object sender, StoreReadDataEventArgs e)
		{
			using (Entities _context = new Entities())
			{
				List<object> rawData;


				if (validateComponentSecurity("SYS.DailyActivity.View"))
				{
					List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
					if (uxTogglePosted.Checked)
					{
						rawData = (from d in _context.DAILY_ACTIVITY_HEADER
								   join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
								   join s in _context.DAILY_ACTIVITY_STATUS on d.STATUS equals s.STATUS
								   join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.HEADER_ID equals eq.HEADER_ID into equ
								   from equip in equ.DefaultIfEmpty()
								   join pr in _context.PROJECTS_V on equip.PROJECT_ID equals pr.PROJECT_ID into pro
								   from proj in pro.DefaultIfEmpty()
								   join em in _context.DAILY_ACTIVITY_EMPLOYEE on d.HEADER_ID equals em.HEADER_ID into emp
								   from empl in emp.DefaultIfEmpty()
								   join empv in _context.EMPLOYEES_V on empl.PERSON_ID equals empv.PERSON_ID into emplv
								   from withempl in emplv.DefaultIfEmpty()
								   where OrgsList.Contains(p.CARRYING_OUT_ORGANIZATION_ID) || OrgsList.Contains(proj.CARRYING_OUT_ORGANIZATION_ID) || OrgsList.Contains((long)withempl.ORGANIZATION_ID)
								   select new { d.HEADER_ID, d.PROJECT_ID, d.DA_DATE, p.SEGMENT1, p.LONG_NAME, s.STATUS_VALUE, d.DA_HEADER_ID, d.STATUS, p.ORG_ID }).Distinct().ToList<object>();
					}
					else
					{
						rawData = (from d in _context.DAILY_ACTIVITY_HEADER
								   join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
								   join s in _context.DAILY_ACTIVITY_STATUS on d.STATUS equals s.STATUS
								   join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.HEADER_ID equals eq.HEADER_ID into equ
								   from equip in equ.DefaultIfEmpty()
								   join pr in _context.PROJECTS_V on equip.PROJECT_ID equals pr.PROJECT_ID into pro
								   from proj in pro.DefaultIfEmpty()
								   join em in _context.DAILY_ACTIVITY_EMPLOYEE on d.HEADER_ID equals em.HEADER_ID into emp
								   from empl in emp.DefaultIfEmpty()
								   join empv in _context.EMPLOYEES_V on empl.PERSON_ID equals empv.PERSON_ID into emplv
								   from withempl in emplv.DefaultIfEmpty()
								   where d.STATUS != 4 && (OrgsList.Contains(p.CARRYING_OUT_ORGANIZATION_ID) || OrgsList.Contains(proj.CARRYING_OUT_ORGANIZATION_ID) || OrgsList.Contains((long)withempl.ORGANIZATION_ID))
								   select new { d.HEADER_ID, d.PROJECT_ID, d.DA_DATE, p.SEGMENT1, p.LONG_NAME, s.STATUS_VALUE, d.DA_HEADER_ID, d.STATUS, p.ORG_ID }).Distinct().ToList<object>();
					}
				}
				else
				{
					string EmployeeName = Authentication.GetClaimValue("EmployeeName", User as ClaimsPrincipal);
					long PersonId = (from d in _context.EMPLOYEES_V
									 where d.EMPLOYEE_NAME == EmployeeName
									 select d.PERSON_ID).Single();

					if (uxTogglePosted.Checked)
					{
						rawData = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
								   join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
								   join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
								   join s in _context.DAILY_ACTIVITY_STATUS on h.STATUS equals s.STATUS
								   where d.PERSON_ID == PersonId
								   select new { d.HEADER_ID, h.PROJECT_ID, h.DA_DATE, p.SEGMENT1, p.LONG_NAME, s.STATUS_VALUE, h.DA_HEADER_ID, h.STATUS }).ToList<object>();
					}
					else
					{
						rawData = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
								   join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
								   join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
								   join s in _context.DAILY_ACTIVITY_STATUS on h.STATUS equals s.STATUS
								   where d.PERSON_ID == PersonId && h.STATUS != 4
								   select new { d.HEADER_ID, h.PROJECT_ID, h.DA_DATE, p.SEGMENT1, p.LONG_NAME, s.STATUS_VALUE, h.DA_HEADER_ID, h.STATUS }).ToList<object>();
					}
					uxCreateActivityButton.Disabled = true;

				}
				List<HeaderData> data = new List<HeaderData>();

				List<EmployeeData> HoursOver24 = ValidationChecks.checkEmployeeTime(24);
				List<EmployeeData> HoursOver14 = ValidationChecks.checkEmployeeTime(14);
				List<long> OverlapProjects = ValidationChecks.employeeTimeOverlapCheck();
				List<long> BusinessUnitProjects = ValidationChecks.EquipmentBusinessUnitCheck();
				List<long> BusinessUnitEmployees = ValidationChecks.EmployeeBusinessUnitCheck();
				
				foreach (dynamic record in rawData)
				{
					string Warning = "Zero";
					string WarningType = string.Empty;

					foreach (EmployeeData OffendingProject in HoursOver14)
					{
						if (OffendingProject.HEADER_ID == record.HEADER_ID)
						{
							Warning = "Warning";
							WarningType = "Over 14 hours logged for an employee <br />";
							break;
						}

					}
					foreach (long OffendingProject in BusinessUnitProjects)
					{
						if (OffendingProject == record.HEADER_ID)
						{
							Warning = "Error";
							WarningType += "Contains Equipment outside of Business Unit.<br />";
							break;
						}
					}

					foreach (long OffendingProject in BusinessUnitEmployees)
					{
						if (OffendingProject == record.HEADER_ID)
						{
							Warning = "Error";
							WarningType += "Contains Employees outside of Business Unit.<br />";
							break;
						}
					}
					foreach (EmployeeData OffendingProject in HoursOver24)
					{
						if (OffendingProject.HEADER_ID == record.HEADER_ID)
						{
							Warning = "Error";
							WarningType += "24 or more hours logged for an employee.<br />";
							break;
						}
					}


					foreach (long OffendingProject in OverlapProjects)
					{
						if (OffendingProject == record.HEADER_ID)
						{
							Warning = "Error";
							WarningType += "An employee has overlapping time with another project.<br />";
							break;
						}
					}
					if (record.ORG_ID == 121)
					{
						List<WarningData> LunchList = ValidationChecks.LunchCheck(record.HEADER_ID);
						if (LunchList.Count > 0)
						{
							Warning = "Error";
							WarningType += "An employee is missing a lunch entry.<br />";
						}
					}
					if (record.ORG_ID == 123)
					{
						if (ValidationChecks.employeeWithShopTimeCheck(record.HEADER_ID))
						{
							Warning = "Error";
							WarningType += "An employee is missing shop time.";
						}
					}

					WarningData PerDiems = ValidationChecks.checkPerDiem(record.HEADER_ID);
					if (PerDiems != null)
					{
						Warning = PerDiems.WarningType;
						WarningType += PerDiems.AdditionalInformation;
					}

					data.Add(new HeaderData
					{
						HEADER_ID = record.HEADER_ID,
						PROJECT_ID = record.PROJECT_ID,
						DA_DATE = record.DA_DATE,
						SEGMENT1 = record.SEGMENT1,
						LONG_NAME = record.LONG_NAME,
						STATUS_VALUE = record.STATUS_VALUE,
						DA_HEADER_ID = record.DA_HEADER_ID,
						STATUS = record.STATUS,
						WARNING = Warning,
						WARNING_TYPE = WarningType
					});
				}


				var SortedData = data.OrderBy(x => x.STATUS).ThenBy(x => x.WARNING).ThenBy(x => x.DA_DATE).ToList<HeaderData>();
				int count;
				uxManageGridStore.DataSource = GenericData.EnumerableFilterHeader<HeaderData>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], SortedData, out count);
				e.Total = count;

			}
		}

		/// <summary>
		/// Update Tab URLs based on selected header and activate buttons
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void deUpdateUrlAndButtons(object sender, DirectEventArgs e)
		{
			long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);
			string homeUrl = string.Empty;
			long OrgId = GetOrgId(HeaderId);
			long CoOrgId = GetCoOrgId(HeaderId);
			List<EmployeeData> HoursOver24 = ValidationChecks.checkEmployeeTime(24);
			bool BadHeader = false;

			if (OrgId == 121)
			{
				homeUrl = string.Format("umCombinedTab_DBI.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
			}
			else if (OrgId == 123)
			{
				homeUrl = string.Format("umCombinedTab_IRM.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
			}

			uxCombinedTab.Disabled = false;
			uxCombinedTab.LoadContent(homeUrl);
			if (SYS_USER_ORGS.IsInOrg(SYS_USER_INFORMATION.UserID(User.Identity.Name), CoOrgId))
			{
				if (validateComponentSecurity("SYS.DailyActivity.View"))
				{
					string prodUrl = string.Empty;
					string headerUrl = string.Format("umHeaderTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
					string equipUrl = string.Format("umEquipmentTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
					string emplUrl = string.Format("umEmployeesTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
					string chemUrl = string.Format("umChemicalTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
					string weatherUrl = string.Format("umWeatherTab.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
					string invUrl = string.Empty;
					string footerURL = string.Empty;

					uxHeaderTab.Disabled = false;
					uxEquipmentTab.Disabled = false;
					uxProductionTab.Disabled = false;
					uxEmployeeTab.Disabled = false;
					uxWeatherTab.Disabled = false;
					uxInventoryTab.Disabled = false;
					uxFooterTab.Disabled = false;

					uxHeaderTab.LoadContent(headerUrl);
					uxEquipmentTab.LoadContent(equipUrl);
					uxEmployeeTab.LoadContent(emplUrl);
					uxWeatherTab.LoadContent(weatherUrl);

					if (OrgId == 121)
					{
						prodUrl = string.Format("umProductionTab_DBI.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
						invUrl = string.Format("umInventoryTab_DBI.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
						footerURL = string.Format("umSubmitActivity_DBI.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
						uxChemicalTab.Disabled = false;
						uxTabPanel.ShowTab(uxChemicalTab);
						uxChemicalTab.LoadContent(chemUrl);
					}
					else if (OrgId == 123)
					{
						uxTabPanel.HideTab(uxChemicalTab);
						prodUrl = string.Format("umProductionTab_IRM.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
						invUrl = string.Format("umInventoryTab_IRM.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
						footerURL = string.Format("umSubmitActivity_IRM.aspx?headerId={0}", e.ExtraParams["HeaderId"]);
						//uxChemicalTab.Close();

					}
					uxProductionTab.LoadContent(prodUrl);
					uxInventoryTab.LoadContent(invUrl);
					uxFooterTab.LoadContent(footerURL);
				}

				switch (e.ExtraParams["Status"])
				{
					case "PENDING APPROVAL":
						uxApproveActivityButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.Approve");
						uxTabApproveButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.Approve");
						uxPostActivityButton.Disabled = true;
						uxTabPostButton.Disabled = true;
						uxPostMultipleButton.Disabled = true;
						uxMarkAsPostedButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.MarkAsPosted");
						uxTabMarkButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.MarkAsPosted");
						uxTabSetInactiveButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.View");
						uxInactiveActivityButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.View");
						uxDeactivate.Value = "Deactivate";
						uxTabSetInactiveButton.Text = "Set Inactive";
						uxInactiveActivityButton.Text = "Set Inactive";
						break;
					case "APPROVED":
						uxTabSetInactiveButton.Text = "Set Inactive";
						uxInactiveActivityButton.Text = "Set Inactive";
						uxPostActivityButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.Post");
						uxTabPostButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.Post");
						uxPostMultipleButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.Post");
						uxMarkAsPostedButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.MarkAsPosted");
						uxTabMarkButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.MarkAsPosted");
						uxTabSetInactiveButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.View");
						uxInactiveActivityButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.View");
						uxDeactivate.Value = "Deactivate";

						uxChemicalTab.Disabled = true;
						uxEmployeeTab.Disabled = true;
						uxEquipmentTab.Disabled = true;
						uxInventoryTab.Disabled = true;
						uxHeaderTab.Disabled = true;
						uxWeatherTab.Disabled = true;
						uxProductionTab.Disabled = true;
						uxFooterTab.Disabled = true;

						if (validateComponentSecurity("SYS.DailyActivity.Post") && validateComponentSecurity("SYS.DailyActivity.Approve"))
						{
							uxApproveActivityButton.Text = "Unapprove";
							uxTabApproveButton.Text = "Unapprove";
							uxApproveActivityButton.Disabled = false;
							uxTabApproveButton.Disabled = false;
						}
						else
						{
							uxApproveActivityButton.Disabled = true;
							uxTabApproveButton.Disabled = true;
						}
						break;
					case "POSTED":
						uxApproveActivityButton.Disabled = true;
						uxTabApproveButton.Disabled = true;
						uxPostActivityButton.Disabled = true;
						uxTabPostButton.Disabled = true;
						uxPostMultipleButton.Disabled = true;
						uxMarkAsPostedButton.Disabled = true;
						uxTabMarkButton.Disabled = true;
						uxTabSetInactiveButton.Disabled = true;
						uxInactiveActivityButton.Disabled = true;
						uxChemicalTab.Disabled = true;
						uxEmployeeTab.Disabled = true;
						uxEquipmentTab.Disabled = true;
						uxInventoryTab.Disabled = true;
						uxHeaderTab.Disabled = true;
						uxWeatherTab.Disabled = true;
						uxProductionTab.Disabled = true;
						uxFooterTab.Disabled = true;
						break;
					case "INACTIVE":
						uxApproveActivityButton.Disabled = true;
						uxTabApproveButton.Disabled = true;
						uxPostActivityButton.Disabled = true;
						uxTabPostButton.Disabled = true;
						uxPostMultipleButton.Disabled = true;
						uxMarkAsPostedButton.Disabled = true;
						uxTabMarkButton.Disabled = true;

						uxTabSetInactiveButton.Text = "Activate";
						uxInactiveActivityButton.Text = "Activate";
						uxTabSetInactiveButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.View");
						uxDeactivate.Value = "Activate";
						uxInactiveActivityButton.Disabled = !validateComponentSecurity("SYS.DailyActivity.View");
						break;

				}

				List<long> EmployeeOverLap = ValidationChecks.employeeTimeOverlapCheck();

				if (HoursOver24.Count > 0)
				{
					if (HoursOver24.Exists(emp => emp.HEADER_ID == HeaderId))
					{
						EmployeeData HeaderData = HoursOver24.Find(emp => emp.HEADER_ID == HeaderId);
						BadHeader = true;
					}


				}

				if (EmployeeOverLap.Count > 0)
				{
					using (Entities _context = new Entities())
					{
						if (EmployeeOverLap.Exists(x => x == HeaderId))
						{
							var HeaderData = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
											  join emp in _context.EMPLOYEES_V on d.PERSON_ID equals emp.PERSON_ID
											  where d.HEADER_ID == HeaderId
											  select new { d.DAILY_ACTIVITY_HEADER.DA_DATE, emp.EMPLOYEE_NAME }).First();
							BadHeader = true;
						}
					}
				}

				if (BadHeader)
				{
					uxApproveActivityButton.Disabled = true;
					uxTabApproveButton.Disabled = true;
					uxPostActivityButton.Disabled = true;
					uxTabPostButton.Disabled = true;
				}
			}
			else
			{
				uxApproveActivityButton.Disabled = true;
				uxTabApproveButton.Disabled = true;
				uxPostActivityButton.Disabled = true;
				uxTabPostButton.Disabled = true;
				uxPostMultipleButton.Disabled = true;
				uxMarkAsPostedButton.Disabled = true;
				uxTabMarkButton.Disabled = true;
				uxTabSetInactiveButton.Disabled = true;
				uxInactiveActivityButton.Disabled = true;
				uxChemicalTab.Disabled = true;
				uxEmployeeTab.Disabled = true;
				uxEquipmentTab.Disabled = true;
				uxInventoryTab.Disabled = true;
				uxHeaderTab.Disabled = true;
				uxWeatherTab.Disabled = true;
				uxProductionTab.Disabled = true;
				uxFooterTab.Disabled = true;
			}

			uxExportToPDF.Disabled = false;
			uxTabExportButton.Disabled = false;
			uxEmailPdf.Disabled = false;
			uxTabEmailButton.Disabled = false;
			uxTabPanel.Expand();
			uxTabPanel.SetActiveTab(uxCombinedTab);
		}

		protected long GetCoOrgId(long HeaderId)
		{
			using (Entities _context = new Entities())
			{
				long CoOrgId = (from d in _context.DAILY_ACTIVITY_HEADER
								join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
								where d.HEADER_ID == HeaderId
								select p.CARRYING_OUT_ORGANIZATION_ID).Single();
				return CoOrgId;
			}
		}

		protected void deLoadNextActivity(object sender, DirectEventArgs e)
		{
			RowSelectionModel GridModel = uxManageGrid.GetSelectionModel() as RowSelectionModel;
			var Index = GridModel.SelectedIndex;
			int LastRecord = int.Parse(e.ExtraParams["ToRecord"].ToString()) - int.Parse(e.ExtraParams["FromRecord"].ToString());
			if (Index < LastRecord)
			{
				GridModel.SelectedRow.RowIndex = GridModel.SelectedIndex + 1;
				GridModel.Select(GridModel.SelectedIndex);
				GridModel.UpdateSelection();
			}
			else if (LastRecord == 19)
			{

				uxManageGridStore.NextPage(new
				{
					callback = JRawValue.From("function() {App.uxManageGrid.getSelectionModel().select(0)}")
				});

			}
		}

		protected void deLoadPreviousActivity(object sender, DirectEventArgs e)
		{
			RowSelectionModel GridModel = uxManageGrid.GetSelectionModel() as RowSelectionModel;
			var Index = GridModel.SelectedIndex;

			if (Index > 0)
			{
				GridModel.SelectedRow.RowIndex = GridModel.SelectedIndex - 1;
				GridModel.Select(GridModel.SelectedIndex);
				GridModel.UpdateSelection();
			}
			else if (int.Parse(e.ExtraParams["CurrentPage"].ToString()) != 1)
			{
				X.Js.Call("App.uxManageGridStore.previousPage({callback: function(){App.uxManageGrid.getSelectionModel().select(19)}})");

			}
		}

		/// <summary>
		/// Set Header to Inactive status(5)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void deSetHeaderInactive(object sender, DirectEventArgs e)
		{
			long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);
			DAILY_ACTIVITY_HEADER data;
			//Get Record to be updated
			using (Entities _context = new Entities())
			{
				data = (from d in _context.DAILY_ACTIVITY_HEADER
						where d.HEADER_ID == HeaderId
						select d).Single();
			}
			Ext.Net.Button MyButton = sender as Ext.Net.Button;
			if (uxDeactivate.Value.ToString() == "Deactivate")
			{
				data.STATUS = 5;

			}
			else
			{
				data.STATUS = 2;
			}
			//Update record in DB
			GenericData.Update<DAILY_ACTIVITY_HEADER>(data);
			RowSelectionModel GridModel = uxManageGrid.GetSelectionModel() as RowSelectionModel;
			var Index = GridModel.SelectedIndex;


			uxManageGridStore.Reload(new
			{
				callback = JRawValue.From("function() {App.uxManageGrid.getSelectionModel().select(" + Index + ")}")
			});

		}

		/// <summary>
		/// Approve Activity(set status to 3)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void deApproveActivity(object sender, DirectEventArgs e)
		{
			long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);

			DAILY_ACTIVITY_HEADER data;

			//Get record to be updated
			using (Entities _context = new Entities())
			{
				data = (from d in _context.DAILY_ACTIVITY_HEADER
						where d.HEADER_ID == HeaderId
						select d).Single();
				data.STATUS = 3;
			}

			//Update record in DB
			GenericData.Update<DAILY_ACTIVITY_HEADER>(data);
			RowSelectionModel GridModel = uxManageGrid.GetSelectionModel() as RowSelectionModel;
			var Index = GridModel.SelectedIndex;


			uxManageGridStore.Reload(new
			{
				callback = JRawValue.From("function() {App.uxManageGrid.getSelectionModel().select(" + Index + ")}")
			});

		}

		/// <summary>
		/// Get Header Information
		/// </summary>
		/// <param name="HeaderId"></param>
		/// <returns></returns>
		protected List<object> GetHeader(long HeaderId)
		{
			using (Entities _context = new Entities())
			{
				var returnData = (from d in _context.DAILY_ACTIVITY_HEADER
								  join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
								  where d.HEADER_ID == HeaderId
								  select new { d.APPLICATION_TYPE, d.CONTRACTOR, d.DA_DATE, d.DENSITY, d.LICENSE, d.STATE, d.STATUS, d.SUBDIVISION, p.SEGMENT1, p.LONG_NAME, d.DA_HEADER_ID }).ToList<object>();
				return returnData;
			}
		}

		/// <summary>
		/// Get Employee/Equipment Information
		/// </summary>
		/// <param name="HeaderId"></param>
		/// <returns></returns>
		protected List<EmployeeDetails> GetEmployee(long HeaderId)
		{
			using (Entities _context = new Entities())
			{
				var returnData = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
								  join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
								  join eq in _context.DAILY_ACTIVITY_EQUIPMENT on d.EQUIPMENT_ID equals eq.EQUIPMENT_ID into equ
								  from equip in equ.DefaultIfEmpty()
								  join p in _context.PROJECTS_V on equip.PROJECT_ID equals p.PROJECT_ID into proj
								  from projects in proj.DefaultIfEmpty()
								  where d.HEADER_ID == HeaderId
								  select new EmployeeDetails { EMPLOYEE_NAME = e.EMPLOYEE_NAME, NAME = projects.NAME, LUNCH = d.LUNCH, LUNCH_LENGTH = d.LUNCH_LENGTH, TIME_IN = (DateTime)d.TIME_IN, TIME_OUT = (DateTime)d.TIME_OUT, FOREMAN_LICENSE = d.FOREMAN_LICENSE, TRAVEL_TIME = (d.TRAVEL_TIME == null ? 0 : d.TRAVEL_TIME), DRIVE_TIME = (d.DRIVE_TIME == null ? 0 : d.DRIVE_TIME), SHOPTIME_AM = (d.SHOPTIME_AM == null ? 0 : d.SHOPTIME_AM), SHOPTIME_PM = (d.SHOPTIME_PM == null ? 0 : d.SHOPTIME_PM), PER_DIEM = d.PER_DIEM, COMMENTS = d.COMMENTS }).ToList();
				foreach (var item in returnData)
				{
					double Hours = Math.Truncate((double)item.TRAVEL_TIME);
					double Minutes = Math.Round(((double)item.TRAVEL_TIME - Hours) * 60);
					TimeSpan TotalTimeSpan = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), 0);
					item.TRAVEL_TIME_FORMATTED = TotalTimeSpan.ToString("hh\\:mm");
					Hours = Math.Truncate((double)item.DRIVE_TIME);
					Minutes = Math.Round(((double)item.DRIVE_TIME - Hours) * 60);
					item.DRIVE_TIME_FORMATTED = TotalTimeSpan.ToString("hh\\:mm");
					Hours = Math.Truncate((double)item.SHOPTIME_AM);
					Minutes = Math.Round(((double)item.SHOPTIME_AM - Hours) * 60);
					item.SHOPTIME_AM_FORMATTED = TotalTimeSpan.ToString("hh\\:mm");
					Hours = Math.Truncate((double)item.SHOPTIME_PM);
					Minutes = Math.Round(((double)item.SHOPTIME_PM - Hours) * 60);
					item.SHOPTIME_PM_FORMATTED = TotalTimeSpan.ToString("hh\\:mm");
				}
				return returnData;
			}
		}

		protected List<EquipmentDetails> GetEquipment(long HeaderId)
		{
			using (Entities _context = new Entities())
			{
				var data = (from e in _context.DAILY_ACTIVITY_EQUIPMENT
							join p in _context.CLASS_CODES_V on e.PROJECT_ID equals p.PROJECT_ID
							where e.HEADER_ID == HeaderId
							select new EquipmentDetails { CLASS_CODE = p.CLASS_CODE, ORGANIZATION_NAME = p.ORGANIZATION_NAME, ODOMETER_START = e.ODOMETER_START, ODOMETER_END = e.ODOMETER_END, PROJECT_ID = e.PROJECT_ID, EQUIPMENT_ID = e.EQUIPMENT_ID, NAME = p.NAME, HEADER_ID = e.HEADER_ID }).ToList();
				return data;
			}
		}
		/// <summary>
		/// Get Production information
		/// </summary>
		/// <param name="HeaderId"></param>
		/// <returns></returns>
		protected List<object> GetProductionDBI(long HeaderId)
		{
			using (Entities _context = new Entities())
			{
				var returnData = (from d in _context.DAILY_ACTIVITY_PRODUCTION
								  join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
								  join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
								  join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
								  where d.HEADER_ID == HeaderId
								  select new { t.TASK_NUMBER, t.DESCRIPTION, d.TIME_IN, d.TIME_OUT, d.WORK_AREA, d.POLE_FROM, d.POLE_TO, d.ACRES_MILE, d.QUANTITY }).ToList<object>();
				return returnData;
			}
		}

		protected List<object> GetProductionIRM(long HeaderId)
		{
			using (Entities _context = new Entities())
			{
				var returnData = (from d in _context.DAILY_ACTIVITY_PRODUCTION
								  join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
								  join t in _context.PA_TASKS_V on d.TASK_ID equals t.TASK_ID
								  join p in _context.PROJECTS_V on h.PROJECT_ID equals p.PROJECT_ID
								  where d.HEADER_ID == HeaderId
								  select new { t.TASK_NUMBER, t.DESCRIPTION, d.WORK_AREA, d.STATION, d.EXPENDITURE_TYPE, d.COMMENTS, d.QUANTITY }).ToList<object>();
				return returnData;
			}
		}
		/// <summary>
		/// Get Weather Information
		/// </summary>
		/// <param name="HeaderId"></param>
		/// <returns></returns>
		protected List<DAILY_ACTIVITY_WEATHER> GetWeather(long HeaderId)
		{
			using (Entities _context = new Entities())
			{
				var returnData = (from d in _context.DAILY_ACTIVITY_WEATHER
								  where d.HEADER_ID == HeaderId
								  select d).ToList();
				return returnData;
			}
		}

		/// <summary>
		/// Get Chemical Mix Information
		/// </summary>
		/// <param name="HeaderId"></param>
		/// <returns></returns>
		protected List<DAILY_ACTIVITY_CHEMICAL_MIX> GetChemicalMix(long HeaderId)
		{
			using (Entities _context = new Entities())
			{
				var returnData = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
								  where d.HEADER_ID == HeaderId
								  select d).ToList();
				return returnData;
			}
		}

		/// <summary>
		/// Get Inventory Information
		/// </summary>
		/// <param name="HeaderId"></param>
		/// <returns></returns>
		protected List<object> GetInventoryDBI(long HeaderId)
		{
			using (Entities _context = new Entities())
			{
				List<object> returnData = (from d in _context.DAILY_ACTIVITY_INVENTORY
										   join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
										   join c in _context.DAILY_ACTIVITY_CHEMICAL_MIX on d.CHEMICAL_MIX_ID equals c.CHEMICAL_MIX_ID
										   join u in _context.UNIT_OF_MEASURE_V on d.UNIT_OF_MEASURE equals u.UOM_CODE
										   where d.HEADER_ID == HeaderId
										   from j in joined
										   where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
										   select new { c.CHEMICAL_MIX_NUMBER, j.INV_NAME, d.SUB_INVENTORY_SECONDARY_NAME, j.DESCRIPTION, d.TOTAL, d.RATE, u.UNIT_OF_MEASURE, d.EPA_NUMBER }).ToList<object>();

				return returnData;
			}
		}

		protected List<object> GetInventoryIRM(long HeaderId)
		{
			using (Entities _context = new Entities())
			{
				List<object> returnData = (from d in _context.DAILY_ACTIVITY_INVENTORY
										   join i in _context.INVENTORY_V on d.ITEM_ID equals i.ITEM_ID into joined
										   join u in _context.UNIT_OF_MEASURE_V on d.UNIT_OF_MEASURE equals u.UOM_CODE
										   where d.HEADER_ID == HeaderId
										   from j in joined
										   where j.ORGANIZATION_ID == d.SUB_INVENTORY_ORG_ID
										   select new { d.SUB_INVENTORY_SECONDARY_NAME, j.INV_NAME, j.DESCRIPTION, d.RATE, u.UNIT_OF_MEASURE }).ToList<object>();

				return returnData;
			}
		}

		/// <summary>
		/// Get footer Information
		/// </summary>
		/// <param name="HeaderId"></param>
		/// <returns></returns>
		protected DAILY_ACTIVITY_FOOTER GetFooter(long HeaderId)
		{
			using (Entities _context = new Entities())
			{
				DAILY_ACTIVITY_FOOTER returnData = (from d in _context.DAILY_ACTIVITY_FOOTER
													where d.HEADER_ID == HeaderId
													select d).SingleOrDefault();
				return returnData;
			}
		}

		/// <summary>
		/// Export selected Header to PDF
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void deExportToPDF(object sender, DirectEventArgs e)
		{
			//Set header Id
			long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);
			string ProjectName;
			using (Entities _context = new Entities())
			{
				ProjectName = (from d in _context.DAILY_ACTIVITY_HEADER
							   join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
							   where d.HEADER_ID == HeaderId
							   select p.LONG_NAME).Single();
			}
			MemoryStream PdfStream = generatePDF(HeaderId);

			Response.Clear();
			Response.ClearContent();
			Response.ClearHeaders();
			Response.ContentType = "application/pdf";
			Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}{1}-export.pdf", HeaderId.ToString(), RemoveSpecialCharacters(ProjectName)));
			Response.BinaryWrite(PdfStream.ToArray());
			Response.End();
		}

		protected static string RemoveSpecialCharacters(string str)
		{
			StringBuilder sb = new StringBuilder();
			foreach (char c in str)
			{
				if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
				{
					sb.Append(c);
				}
			}
			return sb.ToString();
		}

		protected void deSendPDF(object sender, DirectEventArgs e)
		{
			long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);

			using (MemoryStream PdfStream = new MemoryStream(generatePDF(HeaderId).ToArray()))
			{
				string Subject = "Copy of Daily Activity Report";
				bool IsHtml = true;
				string Message = "Please find attached the Daily Activity Report you requested.";

				PdfStream.Position = 0;

				Attachment MailAttachment = new Attachment(PdfStream, HeaderId.ToString() + "-export.pdf");

				Mailer.SendMessage(User.Identity.Name + "@dbiservices.com", Subject, Message, IsHtml, MailAttachment);
			}
		}

		protected void dePostToOracle(object sender, DirectEventArgs e)
		{
			long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);

			try
			{
				Interface.PostToOracle(HeaderId, User.Identity.Name);
			}
			catch (Exception)
			{
				throw;
			}

			Notification.Show(new NotificationConfig()
			{
				Title = "Success",
				Html = "Daily Activity posted successfully",
				HideDelay = 1000,
				AlignCfg = new NotificationAlignConfig
				{
					ElementAnchor = AnchorPoint.Center,
					TargetAnchor = AnchorPoint.Center
				}
			});
			uxManageGridStore.Reload();
		}

		protected void deMarkAsPosted(object sender, DirectEventArgs e)
		{
			long HeaderId = long.Parse(e.ExtraParams["HeaderId"]);
			DAILY_ACTIVITY_HEADER ToUpdate;
			using (Entities _context = new Entities())
			{
				ToUpdate = _context.DAILY_ACTIVITY_HEADER.Where(x => x.HEADER_ID == HeaderId).Single();
				ToUpdate.STATUS = 4;
			}
			GenericData.Update<DAILY_ACTIVITY_HEADER>(ToUpdate);
			uxManageGridStore.Reload();
		}

		protected MemoryStream generatePDF(long HeaderId)
		{
			long OrgId;
			using (Entities _context = new Entities())
			{
				OrgId = (from d in _context.DAILY_ACTIVITY_HEADER
						 join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
						 where d.HEADER_ID == HeaderId
						 select (long)p.ORG_ID).Single();
			}

			using (MemoryStream PdfStream = new MemoryStream())
			{
				//Create the document
				Document ExportedPDF = new Document(iTextSharp.text.PageSize.LETTER, 0f, 0f, 42f, 42f);
				PdfWriter ExportWriter = PdfWriter.GetInstance(ExportedPDF, PdfStream);
				Paragraph NewLine = new Paragraph("\n");
				Font HeaderFont = FontFactory.GetFont("Verdana", 6, Font.BOLD);
				Font HeadFootTitleFont = FontFactory.GetFont("Verdana", 7, Font.BOLD);
				Font HeadFootCellFont = FontFactory.GetFont("Verdana", 7);
				Font CellFont = FontFactory.GetFont("Verdana", 6);
				//Open Document
				ExportedPDF.Open();

				//Get Header Data
				var HeaderData = GetHeader(HeaderId);

				//Create Header Table
				PdfPTable HeaderTable = new PdfPTable(4);
				HeaderTable.DefaultCell.Border = PdfPCell.NO_BORDER;
				PdfPCell[] Cells;
				PdfPRow Row;
				foreach (dynamic Data in HeaderData)
				{
					Paragraph Title = new Paragraph("DAILY ACTIVITY REPORT", FontFactory.GetFont("Verdana", 12, Font.BOLD));
					Title.Alignment = 1;

					ExportedPDF.Add(Title);

					Title = new Paragraph(Data.LONG_NAME, FontFactory.GetFont("Verdana", 12, Font.BOLD));
					Title.Alignment = 1;
					ExportedPDF.Add(Title);

					Title = new Paragraph(Data.DA_DATE.Date.ToString("MM/dd/yyyy"), FontFactory.GetFont("Verdana", 12, Font.BOLD));
					Title.Alignment = 1;
					ExportedPDF.Add(Title);
					ExportedPDF.Add(NewLine);

					string OracleHeader;
					try
					{
						OracleHeader = Data.DA_HEADER_ID.ToString();
					}
					catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
					{
						OracleHeader = "0";
					}
					Cells = new PdfPCell[]{
						new PdfPCell(new Phrase("DRS Id", HeadFootTitleFont )),
						new PdfPCell(new Phrase(HeaderId.ToString(), HeadFootCellFont)),
						new PdfPCell(new Phrase("Oracle Header Id", HeadFootTitleFont)),
						new PdfPCell(new Phrase(OracleHeader, HeadFootCellFont))
					};
					foreach (PdfPCell Cell in Cells)
					{
						Cell.Border = PdfPCell.NO_BORDER;
					}
					Row = new PdfPRow(Cells);
					HeaderTable.Rows.Add(Row);

					//First row
					Cells = new PdfPCell[]{
					new PdfPCell(new Phrase("Project Number", HeadFootTitleFont)),
					new PdfPCell(new Phrase(Data.SEGMENT1.ToString(), HeadFootCellFont)),
					new PdfPCell(new Phrase("Sub-Division", HeadFootTitleFont)),
					new PdfPCell(new Phrase(Data.SUBDIVISION, HeadFootCellFont))};

					foreach (PdfPCell Cell in Cells)
					{
						Cell.Border = PdfPCell.NO_BORDER;
					}
					Row = new PdfPRow(Cells);
					HeaderTable.Rows.Add(Row);

					//Second row
					Cells = new PdfPCell[]{
					new PdfPCell(new Phrase("Business License #", HeadFootTitleFont)),
					new PdfPCell(new Phrase(Data.LICENSE, HeadFootCellFont)),
					new PdfPCell(new Phrase("State", HeadFootTitleFont)),
					new PdfPCell(new Phrase(Data.STATE, HeadFootCellFont))};

					foreach (PdfPCell Cell in Cells)
					{
						Cell.Border = PdfPCell.NO_BORDER;
					}
					Row = new PdfPRow(Cells);
					HeaderTable.Rows.Add(Row);

					//Third row
					Cells = new PdfPCell[]{
					new PdfPCell(new Phrase("Type of Application/Work", HeadFootTitleFont)),
					new PdfPCell(new Phrase(Data.APPLICATION_TYPE, HeadFootCellFont)),
					new PdfPCell(new Phrase("Density", HeadFootTitleFont)),
					new PdfPCell(new Phrase(Data.DENSITY, HeadFootCellFont))};

					foreach (PdfPCell Cell in Cells)
					{
						Cell.Border = PdfPCell.NO_BORDER;
					}
					Row = new PdfPRow(Cells);
					HeaderTable.Rows.Add(Row);
				}
				ExportedPDF.Add(HeaderTable);

				ExportedPDF.Add(NewLine);

				try
				{
					//Get Equipment/Employee Data
					var EmployeeData = GetEmployee(HeaderId);
					if (OrgId == 123)
					{
						PdfPTable EmployeeTable = new PdfPTable(12);

						Cells = new PdfPCell[]{
						new PdfPCell(new Phrase("Truck/Equipment \n Name", HeaderFont)),
						new PdfPCell(new Phrase("Operator(s)", HeaderFont)),
						new PdfPCell(new Phrase("License #", HeaderFont)),
						new PdfPCell(new Phrase("Time\nIn", HeaderFont)),
						new PdfPCell(new Phrase("Time\nOut", HeaderFont)),
						new PdfPCell(new Phrase("Total\nHours", HeaderFont)),
						new PdfPCell(new Phrase("Travel\nTime", HeaderFont)),
						new PdfPCell(new Phrase("Drive\nTime", HeaderFont)),
						new PdfPCell(new Phrase("ShopTime\nAM", HeaderFont)),
						new PdfPCell(new Phrase("ShopTime\nPM", HeaderFont)),
						new PdfPCell(new Phrase("Per\nDiem", HeaderFont)),
						new PdfPCell(new Phrase("Comments", HeaderFont))};

						Row = new PdfPRow(Cells);
						EmployeeTable.Rows.Add(Row);

						foreach (var Data in EmployeeData)
						{
							string TravelTime;
							try
							{
								TravelTime = Data.TRAVEL_TIME_FORMATTED.ToString();
							}
							catch (Exception)
							{
								TravelTime = string.Empty;
							}
							string EquipmentName;
							try
							{
								EquipmentName = Data.NAME.ToString();
							}
							catch (Exception)
							{
								EquipmentName = String.Empty;
							}
							string Comments;
							try
							{
								Comments = Data.COMMENTS.ToString();
							}
							catch (Exception)
							{
								Comments = String.Empty;
							}
							string License;
							try
							{
								License = Data.FOREMAN_LICENSE;
							}
							catch (Exception)
							{
								License = string.Empty;
							}
							TimeSpan TotalHours = DateTime.Parse(Data.TIME_OUT.ToString()) - DateTime.Parse(Data.TIME_IN.ToString());
							Cells = new PdfPCell[]{
						new PdfPCell(new Phrase(EquipmentName , CellFont)),
						new PdfPCell(new Phrase(Data.EMPLOYEE_NAME.ToString(), CellFont)),
						new PdfPCell(new Phrase(License, CellFont)),
						new PdfPCell(new Phrase(Data.TIME_IN.ToString("hh\\:mm"), CellFont)),
						new PdfPCell(new Phrase(Data.TIME_OUT.ToString("hh\\:mm"), CellFont)),
						new PdfPCell(new Phrase(TotalHours.ToString("hh\\:mm"), CellFont)),
						new PdfPCell(new Phrase(Data.TRAVEL_TIME_FORMATTED, CellFont)),
						new PdfPCell(new Phrase(Data.DRIVE_TIME_FORMATTED, CellFont)),
						new PdfPCell(new Phrase(Data.SHOPTIME_AM_FORMATTED, CellFont)),
						new PdfPCell(new Phrase(Data.SHOPTIME_PM_FORMATTED, CellFont)),
						new PdfPCell(new Phrase(Data.PER_DIEM.ToString(), CellFont)),
						new PdfPCell(new Phrase(Comments, CellFont))
					};
							Row = new PdfPRow(Cells);
							EmployeeTable.Rows.Add(Row);
						}
						ExportedPDF.Add(EmployeeTable);
						ExportedPDF.Add(NewLine);
					}
					else
					{
						PdfPTable EmployeeTable = new PdfPTable(9);

						Cells = new PdfPCell[]{
						new PdfPCell(new Phrase("Truck/Equipment \n Name", HeaderFont)),
						new PdfPCell(new Phrase("Operator(s)", HeaderFont)),
						new PdfPCell(new Phrase("License #", HeaderFont)),
						new PdfPCell(new Phrase("Time\nIn", HeaderFont)),
						new PdfPCell(new Phrase("Time\nOut", HeaderFont)),
						new PdfPCell(new Phrase("Total\nHours", HeaderFont)),
						new PdfPCell(new Phrase("Travel\nTime", HeaderFont)),
						new PdfPCell(new Phrase("Per\nDiem", HeaderFont)),
						new PdfPCell(new Phrase("Comments", HeaderFont))};

						Row = new PdfPRow(Cells);
						EmployeeTable.Rows.Add(Row);

						foreach (var Data in EmployeeData)
						{
							string TravelTime;
							try
							{
								TravelTime = Data.TRAVEL_TIME_FORMATTED.ToString();
							}
							catch (Exception)
							{
								TravelTime = string.Empty;
							}
							string EquipmentName;
							try
							{
								EquipmentName = Data.NAME.ToString();
							}
							catch (Exception)
							{
								EquipmentName = String.Empty;
							}
							string Comments;
							try
							{
								Comments = Data.COMMENTS.ToString();
							}
							catch (Exception)
							{
								Comments = String.Empty;
							}
							string License;
							try
							{
								License = Data.FOREMAN_LICENSE;
							}
							catch (Exception)
							{
								License = string.Empty;
							}
							TimeSpan TotalHours = DateTime.Parse(Data.TIME_OUT.ToString()) - DateTime.Parse(Data.TIME_IN.ToString());
							Cells = new PdfPCell[]{
						new PdfPCell(new Phrase(EquipmentName , CellFont)),
						new PdfPCell(new Phrase(Data.EMPLOYEE_NAME.ToString(), CellFont)),
						new PdfPCell(new Phrase(License, CellFont)),
						new PdfPCell(new Phrase(Data.TIME_IN.ToString("hh\\:mm tt"), CellFont)),
						new PdfPCell(new Phrase(Data.TIME_OUT.ToString("hh\\:mm tt"), CellFont)),
						new PdfPCell(new Phrase(TotalHours.ToString("hh\\:mm"), CellFont)),
						new PdfPCell(new Phrase(Data.TRAVEL_TIME_FORMATTED, CellFont)),
						new PdfPCell(new Phrase(Data.PER_DIEM.ToString(), CellFont)),
						new PdfPCell(new Phrase(Comments, CellFont))
					};
							Row = new PdfPRow(Cells);
							EmployeeTable.Rows.Add(Row);
						}
						ExportedPDF.Add(EmployeeTable);
						ExportedPDF.Add(NewLine);
					}
				}
				catch (Exception)
				{

				}

				try
				{
					//Get Equipment Data
					var EquipmentData = GetEquipment(HeaderId);
					PdfPTable EquipmentTable = new PdfPTable(5);

					Cells = new PdfPCell[]{
						new PdfPCell(new Phrase("Equipment Name", HeaderFont)),
						new PdfPCell(new Phrase("Class Code", HeaderFont)),
						new PdfPCell(new Phrase("Organization Name", HeaderFont)),
						new PdfPCell(new Phrase("Starting Units", HeaderFont)),
						new PdfPCell(new Phrase("Ending Units", HeaderFont))
					};

					Row = new PdfPRow(Cells);
					EquipmentTable.Rows.Add(Row);

					foreach (EquipmentDetails Equipment in EquipmentData)
					{
						string OdometerStart;
						string OdometerEnd;
						string ProjectId;
						try
						{
							OdometerStart = Equipment.ODOMETER_START.ToString();
						}
						catch (Exception)
						{
							OdometerStart = string.Empty;
						}
						try
						{
							OdometerEnd = Equipment.ODOMETER_END.ToString();
						}
						catch (Exception)
						{
							OdometerEnd = string.Empty;
						}

						Cells = new PdfPCell[]{
							new PdfPCell(new Phrase(Equipment.NAME, CellFont)),
							new PdfPCell(new Phrase(Equipment.CLASS_CODE, CellFont)),
							new PdfPCell(new Phrase(Equipment.ORGANIZATION_NAME, CellFont)),
							new PdfPCell(new Phrase(OdometerStart, CellFont)),
							new PdfPCell(new Phrase(OdometerEnd, CellFont))
						};

						Row = new PdfPRow(Cells);
						EquipmentTable.Rows.Add(Row);
					}
					ExportedPDF.Add(EquipmentTable);
					ExportedPDF.Add(NewLine);
				}
				catch (Exception)
				{

				}
				try
				{
					//Get Production Data
					if (OrgId == 121)
					{
						string WorkArea;
						var ProductionData = GetProductionDBI(HeaderId);
						PdfPTable ProductionTable = new PdfPTable(7);

						Cells = new PdfPCell[]{
							new PdfPCell(new Phrase("Task Number", HeaderFont)),
							new PdfPCell(new Phrase("Task Name", HeaderFont)),
							new PdfPCell(new Phrase("Spray/Work Area", HeaderFont)),
							new PdfPCell(new Phrase("Pole/MP\nFrom", HeaderFont)),
							new PdfPCell(new Phrase("Pole/MP\nTo", HeaderFont)),
							new PdfPCell(new Phrase("Acres/Mile", HeaderFont)),
							new PdfPCell(new Phrase("Gallons", HeaderFont))
						};

						Row = new PdfPRow(Cells);
						ProductionTable.Rows.Add(Row);

						foreach (dynamic Data in ProductionData)
						{
							try
							{
								WorkArea = Data.WORK_AREA.ToString();
							}
							catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
							{
								WorkArea = string.Empty;
							}
							string PoleFrom;
							string PoleTo;
							try
							{
								PoleFrom = Data.POLE_FROM.ToString();
							}
							catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
							{
								PoleFrom = String.Empty;
							}
							try
							{
								PoleTo = Data.POLE_TO.ToString();
							}
							catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
							{
								PoleTo = String.Empty;
							}
							Cells = new PdfPCell[]{
								new PdfPCell(new Phrase(Data.TASK_NUMBER, CellFont)),
								new PdfPCell(new Phrase(Data.DESCRIPTION, CellFont)),
								new PdfPCell(new Phrase(WorkArea, CellFont)),
								new PdfPCell(new Phrase(PoleFrom, CellFont)),
								new PdfPCell(new Phrase(PoleTo, CellFont)),
								new PdfPCell(new Phrase(Data.ACRES_MILE.ToString(), CellFont)),
								new PdfPCell(new Phrase(Data.QUANTITY.ToString(), CellFont))
							};

							Row = new PdfPRow(Cells);
							ProductionTable.Rows.Add(Row);
						}
						ExportedPDF.Add(ProductionTable);
					}
					if (OrgId == 123)
					{
						var ProductionData = GetProductionIRM(HeaderId);
						PdfPTable ProductionTable = new PdfPTable(6);


						Cells = new PdfPCell[]{
							new PdfPCell(new Phrase("Task Number", HeaderFont)),
							new PdfPCell(new Phrase("Task Name", HeaderFont)),
							new PdfPCell(new Phrase("Quantity", HeaderFont)),
							new PdfPCell(new Phrase("Station", HeaderFont)),
							new PdfPCell(new Phrase("Expenditure Type", HeaderFont)),
							new PdfPCell(new Phrase("Comments", HeaderFont))
						};

						Row = new PdfPRow(Cells);
						ProductionTable.Rows.Add(Row);

						foreach (dynamic Data in ProductionData)
						{
							Cells = new PdfPCell[]{
								new PdfPCell(new Phrase(Data.TASK_NUMBER, CellFont)),
								new PdfPCell(new Phrase(Data.DESCRIPTION, CellFont)),
								new PdfPCell(new Phrase(Data.QUANTITY.ToString(), CellFont)),
								new PdfPCell(new Phrase(Data.STATION, CellFont)),
								new PdfPCell(new Phrase(Data.EXPENDITURE_TYPE.ToString(), CellFont)),
								new PdfPCell(new Phrase(Data.COMMENTS.ToString(), CellFont))
							};

							Row = new PdfPRow(Cells);
							ProductionTable.Rows.Add(Row);
						}
						ExportedPDF.Add(ProductionTable);
					}
					ExportedPDF.Add(NewLine);
				}
				catch (Exception)
				{

				}
				//Get Weather
				try
				{
					var WeatherData = GetWeather(HeaderId);

					PdfPTable WeatherTable = new PdfPTable(6);

					Cells = new PdfPCell[]{
					new PdfPCell(new Phrase("Time", HeaderFont)),
					new PdfPCell(new Phrase("Wind\nDirection", HeaderFont)),
					new PdfPCell(new Phrase("Wind\nVelocity", HeaderFont)),
					new PdfPCell(new Phrase("Temperature", HeaderFont)),
					new PdfPCell(new Phrase("Humidity", HeaderFont)),
					new PdfPCell(new Phrase("Comments", HeaderFont))
				};

					Row = new PdfPRow(Cells);
					WeatherTable.Rows.Add(Row);

					foreach (dynamic Weather in WeatherData)
					{
						Cells = new PdfPCell[]{
						new PdfPCell(new Phrase(Weather.WEATHER_DATE_TIME.ToString(), CellFont)),
						new PdfPCell(new Phrase(Weather.WIND_DIRECTION, CellFont)),
						new PdfPCell(new Phrase(Weather.WIND_VELOCITY, CellFont)),
						new PdfPCell(new Phrase(Weather.TEMP, CellFont)),
						new PdfPCell(new Phrase(Weather.HUMIDITY, CellFont)),
						new PdfPCell(new Phrase(Weather.COMMENTS, CellFont))
					};

						Row = new PdfPRow(Cells);
						WeatherTable.Rows.Add(Row);
					}
					ExportedPDF.Add(WeatherTable);
					ExportedPDF.Add(NewLine);
				}
				catch (Exception)
				{

				}
				if (OrgId == 121)
				{
					try
					{
						//Get Chemical Mix Data
						var ChemicalData = GetChemicalMix(HeaderId);

						PdfPTable ChemicalTable = new PdfPTable(11);

						Cells = new PdfPCell[]{
					new PdfPCell(new Phrase("Mix #", HeaderFont)),
					new PdfPCell(new Phrase("Target\nArea", HeaderFont)),
					new PdfPCell(new Phrase("Gals/Acre", HeaderFont)),
					new PdfPCell(new Phrase("Gals\nStarting", HeaderFont)),
					new PdfPCell(new Phrase("Gals\nMixed", HeaderFont)),
					new PdfPCell(new Phrase("Total\nGallons", HeaderFont)),
					new PdfPCell(new Phrase("Gals\nRemaining", HeaderFont)),
					new PdfPCell(new Phrase("Gals\nUsed", HeaderFont)),
					new PdfPCell(new Phrase("Acres\nSprayed", HeaderFont)),
					new PdfPCell(new Phrase("State", HeaderFont)),
					new PdfPCell(new Phrase("County", HeaderFont))
				};
						Row = new PdfPRow(Cells);
						ChemicalTable.Rows.Add(Row);

						foreach (dynamic Data in ChemicalData)
						{
							decimal TotalGallons = Data.GALLON_STARTING + Data.GALLON_MIXED;
							decimal GallonsUsed = TotalGallons - Data.GALLON_REMAINING;

							Cells = new PdfPCell[]{
						new PdfPCell(new Phrase(Data.CHEMICAL_MIX_NUMBER != null ? Data.CHEMICAL_MIX_NUMBER.ToString() : string.Empty, CellFont)),
						new PdfPCell(new Phrase(Data.TARGET_AREA != null ? Data.TARGET_AREA : string.Empty, CellFont)),
						new PdfPCell(new Phrase(Data.GALLON_ACRE != null ? Data.GALLON_ACRE.ToString() : string.Empty, CellFont)),
						new PdfPCell(new Phrase(Data.GALLON_STARTING != null ? Data.GALLON_STARTING.ToString() : string.Empty, CellFont)),
						new PdfPCell(new Phrase(Data.GALLON_MIXED != null ? Data.GALLON_MIXED.ToString() : string.Empty, CellFont)),
						new PdfPCell(new Phrase(TotalGallons.ToString(), CellFont)),
						new PdfPCell(new Phrase(Data.GALLON_REMAINING != null ? Data.GALLON_REMAINING.ToString() : string.Empty, CellFont)),
						new PdfPCell(new Phrase(GallonsUsed.ToString(), CellFont)),
						new PdfPCell(new Phrase(Data.ACRES_SPRAYED != null ? Data.ACRES_SPRAYED.ToString() : string.Empty, CellFont)),
						new PdfPCell(new Phrase(Data.STATE != null ? Data.STATE : string.Empty, CellFont)),
						new PdfPCell(new Phrase(Data.COUNTY != null ? Data.COUNTY : string.Empty, CellFont))
					};
							Row = new PdfPRow(Cells);
							ChemicalTable.Rows.Add(Row);
						}

						ExportedPDF.Add(ChemicalTable);
						ExportedPDF.Add(NewLine);
					}
					catch (Exception)
					{
					}
				}

				//Get Inventory Data
				try
				{
					if (OrgId == 121)
					{
						var InventoryData = GetInventoryDBI(HeaderId);
						PdfPTable InventoryTable = new PdfPTable(7);

						Cells = new PdfPCell[]{
					new PdfPCell(new Phrase("Mix #", HeaderFont)),
					new PdfPCell(new Phrase("Inventory Org", HeaderFont)),
					new PdfPCell(new Phrase("Sub-Inventory", HeaderFont)),
					new PdfPCell(new Phrase("Item Name", HeaderFont)),
					new PdfPCell(new Phrase("Rate", HeaderFont)),
					new PdfPCell(new Phrase("EPA \n Number", HeaderFont)),
					new PdfPCell(new Phrase("Total", HeaderFont))
				};
						Row = new PdfPRow(Cells);
						InventoryTable.Rows.Add(Row);

						foreach (dynamic Data in InventoryData)
						{
							string EPANumber;
							try
							{
								EPANumber = Data.EPA_NUMBER;
							}
							catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
							{
								EPANumber = string.Empty;
							}
							Cells = new PdfPCell[]{
						new PdfPCell(new Phrase(Data.CHEMICAL_MIX_NUMBER.ToString(), CellFont)),
						new PdfPCell(new Phrase(Data.INV_NAME, CellFont)),
						new PdfPCell(new Phrase(Data.SUB_INVENTORY_SECONDARY_NAME, CellFont)),
						new PdfPCell(new Phrase(Data.DESCRIPTION, CellFont)),
						new PdfPCell(new Phrase(string.Format("{0} {1}", Data.RATE.ToString(), Data.UNIT_OF_MEASURE), CellFont)),
						new PdfPCell(new Phrase(EPANumber, CellFont)),
						new PdfPCell(new Phrase(Data.TOTAL.ToString(), CellFont))
					};

							Row = new PdfPRow(Cells);
							InventoryTable.Rows.Add(Row);
						}

						ExportedPDF.Add(InventoryTable);
					}
					if (OrgId == 123)
					{
						var InventoryData = GetInventoryIRM(HeaderId);
						PdfPTable InventoryTable = new PdfPTable(4);

						Cells = new PdfPCell[]{
							new PdfPCell(new Phrase("Inventory Org", HeaderFont)),
							new PdfPCell(new Phrase("Sub-Inventory", HeaderFont)),
							new PdfPCell(new Phrase("Item Name", HeaderFont)),
							new PdfPCell(new Phrase("Quantity", HeaderFont)),
					 };
						Row = new PdfPRow(Cells);
						InventoryTable.Rows.Add(Row);

						foreach (dynamic Data in InventoryData)
						{
							Cells = new PdfPCell[]{
								new PdfPCell(new Phrase(Data.INV_NAME, CellFont)),
								new PdfPCell(new Phrase(Data.SUB_INVENTORY_SECONDARY_NAME, CellFont)),
								new PdfPCell(new Phrase(Data.DESCRIPTION, CellFont)),
								new PdfPCell(new Phrase(string.Format("{0} {1}", Data.RATE.ToString(), Data.UNIT_OF_MEASURE), CellFont)),
					};

							Row = new PdfPRow(Cells);
							InventoryTable.Rows.Add(Row);
						}

						ExportedPDF.Add(InventoryTable);
					}
					ExportedPDF.Add(NewLine);
				}
				catch (Exception)
				{

				}
				//Get Footer Data
				try
				{
					var FooterData = GetFooter(HeaderId);
					string ForemanName;
					using (Entities _context = new Entities())
					{
						ForemanName = (from d in _context.DAILY_ACTIVITY_HEADER
									   join e in _context.EMPLOYEES_V on d.PERSON_ID equals e.PERSON_ID
									   where d.HEADER_ID == HeaderId
									   select e.EMPLOYEE_NAME).Single();
									  
					}

					PdfPTable FooterTable = new PdfPTable(4);
					FooterTable.DefaultCell.Border = PdfPCell.NO_BORDER;

					string ReasonForNoWork;
					string Hotel;
					string City;
					string State;
					string Phone;

					try
					{
						ReasonForNoWork = FooterData.COMMENTS;
					}
					catch (NullReferenceException)
					{
						ReasonForNoWork = string.Empty;
					}

					try
					{
						Hotel = FooterData.HOTEL_NAME;
					}
					catch (NullReferenceException)
					{
						Hotel = string.Empty;
					}

					try
					{
						City = FooterData.HOTEL_CITY;
					}
					catch (NullReferenceException)
					{
						City = string.Empty;
					}

					try
					{
						State = FooterData.HOTEL_STATE;
					}
					catch (NullReferenceException)
					{
						State = string.Empty;
					}

					try
					{
						Phone = FooterData.HOTEL_PHONE;
					}
					catch (NullReferenceException)
					{
						Phone = string.Empty;
					}

					Cells = new PdfPCell[] {
					new PdfPCell(new Phrase("Reason for no work", HeadFootTitleFont)),
					new PdfPCell(new Phrase(ReasonForNoWork, HeadFootCellFont)),
					new PdfPCell(new Phrase("Hotel, City, State, & Phone", HeadFootTitleFont)),
					new PdfPCell(new Phrase(string.Format("{0} {1} {2} {3}",Hotel, City, State, Phone ), HeadFootCellFont))
				};

					foreach (PdfPCell Cell in Cells)
					{
						Cell.Border = PdfPCell.NO_BORDER;
					}
					Row = new PdfPRow(Cells);
					FooterTable.Rows.Add(Row);

					Cells = new PdfPCell[]{
						new PdfPCell(new Phrase("Foreman Name", HeadFootTitleFont)),
						new PdfPCell(new Phrase(ForemanName, HeadFootCellFont)),
						new PdfPCell(new Phrase("Contract Rep Name", HeadFootTitleFont)),
						new PdfPCell(new Phrase(FooterData.CONTRACT_REP_NAME, HeadFootCellFont))
					};
					foreach (PdfPCell Cell in Cells)
					{
						Cell.Border = PdfPCell.NO_BORDER;
					}
					Row = new PdfPRow(Cells);
					FooterTable.Rows.Add(Row);

					
					ExportedPDF.Add(FooterTable);

					PdfPTable SignatureTable = new PdfPTable(2);
					iTextSharp.text.Image ForemanImage;
					iTextSharp.text.Image ContractImage;
					try
					{
						ForemanImage = iTextSharp.text.Image.GetInstance(FooterData.FOREMAN_SIGNATURE.ToArray());
						ForemanImage.ScaleAbsolute(250f, 82f);
					}
					catch (Exception)
					{
						ForemanImage = iTextSharp.text.Image.GetInstance(Server.MapPath("/Resources/Images") + "/1pixel.jpg");
					}

					try
					{
						ContractImage = iTextSharp.text.Image.GetInstance(FooterData.CONTRACT_REP.ToArray());
						ContractImage.ScaleAbsolute(250f, 82f);
					}
					catch (Exception)
					{
						ContractImage = iTextSharp.text.Image.GetInstance(Server.MapPath("/Resources/Images") + "/1pixel.jpg");
					}


					Cells = new PdfPCell[]{
					//new PdfPCell(new Phrase("Foreman Signature", HeadFootTitleFont)),
					new PdfPCell(ForemanImage),
					new PdfPCell(ContractImage)
					
				};
					foreach (PdfPCell Cell in Cells)
					{
						Cell.Border = PdfPCell.NO_BORDER;
					}
					Row = new PdfPRow(Cells);
					SignatureTable.Rows.Add(Row);
					//Cells = new PdfPCell[]{
					//    new PdfPCell(new Phrase("Contract Representative", HeadFootTitleFont)),
					//    new PdfPCell(ContractImage)
					//};
					//foreach (PdfPCell Cell in Cells)
					//{
					//    Cell.Border = PdfPCell.NO_BORDER;
					//}
					//Row = new PdfPRow(Cells);
					//SignatureTable.Rows.Add(Row);
					if (OrgId == 123)
					{
						iTextSharp.text.Image DotRepImage;
						try
						{
							DotRepImage = iTextSharp.text.Image.GetInstance(FooterData.DOT_REP.ToArray());
							DotRepImage.ScaleAbsolute(300f, 100f);
						}
						catch (Exception)
						{
							DotRepImage = iTextSharp.text.Image.GetInstance(Server.MapPath("/Resources/Images") + "/1pixel.jpg");
						}

						Cells = new PdfPCell[]{
					
					new PdfPCell(new Phrase("Name", HeadFootTitleFont)),
					new PdfPCell(new Phrase(FooterData.DOT_REP_NAME, HeadFootCellFont))
					};
						foreach (PdfPCell Cell in Cells)
						{
							Cell.Border = PdfPCell.NO_BORDER;
						}
						Row = new PdfPRow(Cells);
						Cells = new PdfPCell[]{
							new PdfPCell(new Phrase("DOT Representative", HeadFootTitleFont)),
							new PdfPCell(DotRepImage)
						};
						foreach (PdfPCell Cell in Cells)
						{
							Cell.Border = PdfPCell.NO_BORDER;
						}
						Row = new PdfPRow(Cells);
						SignatureTable.Rows.Add(Row);
						
					}
					ExportedPDF.Add(SignatureTable);

				}
				catch (Exception)
				{

				}
				//Close Stream and start Download
				ExportWriter.CloseStream = false;
				ExportedPDF.Close();
				return PdfStream;
			}
		}
				

		/// <summary>
		/// DirectMethod accessed from umSubmitActivity.aspx when signature is missing on SubmitActivity form
		/// </summary>
		[DirectMethod]
		public void dmSubmitNotification()
		{
			Notification.Show(new NotificationConfig()
			{
				Title = "Signature Missing",
				Html = "Unable to submit, signature missing.  Please provide the foreman signature.",
				HideDelay = 1000,
				AlignCfg = new NotificationAlignConfig
				{
					ElementAnchor = AnchorPoint.Center,
					TargetAnchor = AnchorPoint.Center
				}
			});
		}

		/// <summary>
		/// DirectMethod accessed from umDailyActivity.aspx after it's been submitted
		/// </summary>
		[DirectMethod]
		public void dmHideAddWindow()
		{
			uxCreateActivityWindow.Hide();
			uxManageGridStore.Reload();
		}

		/// <summary>
		/// Direct Method accessed from umSubmitActivity.aspx after it's submitted
		/// </summary>
		[DirectMethod]
		public void dmHideWindowUpdateGrid()
		{
			uxPlaceholderWindow.Hide();
			var GridModel = uxManageGrid.GetSelectionModel() as RowSelectionModel;
			var GridIndex = GridModel.SelectedIndex;
			uxManageGridStore.Reload();
		}

		/// <summary>
		/// Load create activity form and display the window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void deLoadCreateActivity(object sender, DirectEventArgs e)
		{
			uxCreateActivityWindow.LoadContent();
			uxCreateActivityWindow.Show();
		}

		protected void deOpenPostMultipleWindow(object sender, DirectEventArgs e)
		{
			uxPlaceholderWindow.ClearContent();
			uxPlaceholderWindow.LoadContent("umPostMultipleWindow.aspx");
			uxPlaceholderWindow.Show();
		}

		[DirectMethod]
		public void dmLoadChemicalWindow(string WindowType, string HeaderId, string ChemicalMixId)
		{
			uxPlaceholderWindow.ClearContent();
			uxPlaceholderWindow.LoadContent(string.Format("umAddEditChemical.aspx?HeaderId={0}&Type={1}&ChemicalMixId={2}", HeaderId, WindowType, ChemicalMixId));
			uxPlaceholderWindow.Show();
		}

		[DirectMethod]
		public void dmLoadEmployeeWindow(string WindowType, string HeaderId, string EmployeeId)
		{
			uxPlaceholderWindow.ClearContent();
			uxPlaceholderWindow.LoadContent(string.Format("umAddEditEmployee.aspx?HeaderId={0}&Type={1}&EmployeeId={2}", HeaderId, WindowType, EmployeeId));
			uxPlaceholderWindow.Show();
		}

		[DirectMethod]
		public void dmLoadEquipmentWindow(string WindowType, string HeaderId, string EquipmentId)
		{
			uxPlaceholderWindow.ClearContent();
			uxPlaceholderWindow.LoadContent(string.Format("umAddEditEquipment.aspx?HeaderId={0}&Type={1}&EquipmentId={2}", HeaderId, WindowType, EquipmentId));
			uxPlaceholderWindow.Show();
		}

		[DirectMethod]
		public void dmLoadInventoryWindow_DBI(string WindowType, string HeaderId, string InventoryId)
		{
			uxPlaceholderWindow.ClearContent();
			uxPlaceholderWindow.LoadContent(string.Format("umAddEditInventory_DBI.aspx?HeaderId={0}&Type={1}&InventoryId={2}", HeaderId, WindowType, InventoryId));
			uxPlaceholderWindow.Show();
		}

		[DirectMethod]
		public void dmLoadInventoryWindow_IRM(string WindowType, string HeaderId, string InventoryId)
		{
			uxPlaceholderWindow.ClearContent();
			uxPlaceholderWindow.LoadContent(string.Format("umAddEditInventory_IRM.aspx?HeaderId={0}&Type={1}&InventoryId={2}", HeaderId, WindowType, InventoryId));
			uxPlaceholderWindow.Show();
		}

		[DirectMethod]
		public void dmLoadProductionWindow_DBI(string WindowType, string HeaderId, string ProductionId)
		{
			uxPlaceholderWindow.ClearContent();
			uxPlaceholderWindow.LoadContent(string.Format("umAddEditProduction_DBI.aspx?HeaderId={0}&Type={1}&ProductionId={2}", HeaderId, WindowType, ProductionId));
			uxPlaceholderWindow.Show();
		}

		[DirectMethod]
		public void dmLoadProductionWindow_IRM(string WindowType, string HeaderId, string ProductionId)
		{
			uxPlaceholderWindow.ClearContent();
			uxPlaceholderWindow.LoadContent(string.Format("umAddEditProduction_IRM.aspx?HeaderId={0}&Type={1}&ProductionId={2}", HeaderId, WindowType, ProductionId));
			uxPlaceholderWindow.Show();
		}

		[DirectMethod]
		public void dmLoadWeatherWindow(string WindowType, string HeaderId, string WeatherId)
		{
			uxPlaceholderWindow.ClearContent();
			uxPlaceholderWindow.LoadContent(string.Format("umAddEditWeather.aspx?HeaderId={0}&Type={1}&WeatherId={2}", HeaderId, WindowType, WeatherId));
			uxPlaceholderWindow.Show();
		}

		[DirectMethod]
		public void dmLoadLunchWindow(string HeaderId, string EmployeeId)
		{
			uxPlaceholderWindow.ClearContent();
			uxPlaceholderWindow.LoadContent(string.Format("umChooseLunchHeader.aspx?HeaderId={0}&EmployeeId={1}", HeaderId, EmployeeId));
			uxPlaceholderWindow.Show();
		}

		[DirectMethod]
		public void dmLoadPerDiemWindow(string HeaderId, string EmployeeId)
		{
			uxPlaceholderWindow.ClearContent();
			uxPlaceholderWindow.LoadContent(string.Format("umChoosePerDiem.aspx?HeaderId={0}&EmployeeId={1}", HeaderId, EmployeeId));
			uxPlaceholderWindow.Show();
		}

		[DirectMethod]
		public void dmLoadSupportProjectWindow(string HeaderId, string EmployeeId)
		{
			uxPlaceholderWindow.ClearContent();
			uxPlaceholderWindow.LoadContent(string.Format("umChooseSupportProject.aspx?HeaderId={0}&EmployeeId={1}", HeaderId, EmployeeId));
			uxPlaceholderWindow.Show();
		}
		
	}

	public class EquipmentDetails
	{
		public string SEGMENT1 { get; set; }
		public string CLASS_CODE { get; set; }
		public string ORGANIZATION_NAME { get; set; }
		public long? ODOMETER_START { get; set; }
		public long? ODOMETER_END { get; set; }
		public long? PROJECT_ID { get; set; }
		public long EQUIPMENT_ID { get; set; }
		public string NAME { get; set; }
		public long HEADER_ID { get; set; }
	}

}