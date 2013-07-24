-- ****** Object: View XXEMS.SYS_USER_INFORMATION Script Date: 7/2/2013 11:12:53 AM ******
CREATE NOFORCE VIEW "SYS_USER_INFORMATION"
  AS select a.user_id,a.user_name,(e.first_name || ' ' ||e.middle_names || ' ' || e.last_name) as employee_name,a.email_address,c.name as JOB_NAME,d.name as current_organization,
case when (sysdate between a.start_date and nvl(a.end_date,'31-DEC-4712')) then 'A'ELSE 'I' end as oracle_account_status from apps.fnd_user a
left outer join apps.PER_ASSIGNMENTS_X b on a.employee_id = b.person_id
left outer join apps.per_jobs c on c.job_id = b.job_id
left outer join apps.hr_all_organization_units d on d.organization_id = b.organization_id
left outer join apps.per_people_x e on e.person_id = a.employee_id
where a.employee_id is not null
/
