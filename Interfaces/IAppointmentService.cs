﻿using AmazeCare.Models;
using AmazeCare.Models.DTOs;

namespace AmazeCare.Services
{
    public interface IAppointmentService
    {
        Task<Appointment> ScheduleAppointmentAsync(AppointmentDto dto);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId);
        Task<bool> DeleteAppointmentAsync(int appointmentId);

        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId); //  NEW
        Task<Appointment?> GetAppointmentByIdAsync(int appointmentId);                //  NEW
        Task<Appointment?> UpdateAppointmentAsync(AppointmentDto dto); //new
    }
}

