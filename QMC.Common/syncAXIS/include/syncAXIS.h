#ifndef SYNCAXIS_H
#define SYNCAXIS_H
/*****************************************************************************/
/* Import header for syncAXIS version 1.6 */
/*                                      
 * Manufacturer
 * 
 *    SCANLAB GmbH
 *    Siemensstr. 2a
 *    82178 Puchheim
 *    Germany
 * 
 *    Tel. + 49 (89) 800 746-0
 *    Fax: + 49 (89) 800 746-199
 * 
 *    info@scanlab.de
 *    www.scanlab.de
 * 
 */
/*****************************************************************************/

#include <stdint.h>
#include "syncAXISDefinitions.h"

#ifdef _WIN32
#define DLLAPI   __declspec(dllimport)
#else
#define DLLAPI
#endif

#ifdef __cplusplus
extern "C"
{
#endif
/*****************************************************************************/

struct VersionInfo
{
    uint32_t Major;
    uint32_t Minor;
    uint32_t Revision;
};
typedef struct VersionInfo VersionInfo;

/*! \brief Returns version info on the currently running syncAXIS-DLL. */
DLLAPI VersionInfo slsc_cfg_get_sync_axis_version(void);


//***************************************************************************//
// cfg_* configuration functions
//***************************************************************************//

//***************************************************************************//
//! \brief Resets the specified syncAXIS control instance (after slsc_cfg_release_stage) to a fully functional state. RTC6 board and positioning stage are acquired back again.
//! \note Deprecated - use slsc_ctrl_follow instead.
DLLAPI uint32_t slsc_cfg_acquire_stage(size_t Handle);
//***************************************************************************//
//! \brief Destroys the currently running syncAXIS-DLL instance. In the process, the resources (RTC board, positioning stage, ...) are released.
DLLAPI uint32_t slsc_cfg_delete(size_t Handle);
//***************************************************************************//
//! \brief Auxiliary function for software development: deletes the trajectory configuration object, see row Code example.
DLLAPI uint32_t slsc_cfg_delete_trajectory_config(slsc_TrajectoryConfig** TrajConfig);
//***************************************************************************//
//! \brief Returns the operation mode of the currently running syncAXIS-DLL instance.
DLLAPI uint32_t slsc_cfg_get_mode(size_t Handle, slsc_OperationMode* Mode);
//***************************************************************************//
//! \brief Returns the status of the currently running syncAXIS-DLL instance.
DLLAPI uint32_t slsc_cfg_get_operation_status(size_t Handle, slsc_OperationStatus* State);
//***************************************************************************//
//! \brief Returns the trajectory planning configuration of the currently running syncAXIS-DLL instance.
DLLAPI uint32_t slsc_cfg_get_trajectory_config(size_t Handle, slsc_TrajectoryConfig** TrajConfig);
//***************************************************************************//
//! \brief Creates a new syncAXIS control instance in simulation mode from an existing instance and assigns a unique Handle value to it.
DLLAPI uint32_t slsc_cfg_initialize_copy(size_t* Handle, size_t OriginalHandle);
//***************************************************************************//
//! \brief Creates (by using the specified configuration file) a new syncAXIS control instance and assigns a unique Handle value to it.
DLLAPI uint32_t slsc_cfg_initialize_from_file(size_t* Handle, const char* XmlConfigFileName);
//***************************************************************************//
//! \brief Sets up that a user-supplied function ("callback function") is called with each event of type "job_end_planned".
DLLAPI uint32_t slsc_cfg_register_callback_job_end_planned(size_t Handle, slsc_ExecTimeCallback Callback, void* Context);
//***************************************************************************//
//! \brief Sets up that a user-supplied function ("callback function") is called with each event of type "job_finished_executing".
DLLAPI uint32_t slsc_cfg_register_callback_job_finished_executing(size_t Handle, slsc_ExecTimeCallback Callback, void* Context);
//***************************************************************************//
//! \brief Sets up that a user-supplied function ("callback function") is called with each event of type "job_is_executing".
DLLAPI uint32_t slsc_cfg_register_callback_job_is_executing(size_t Handle, slsc_ExecTimeCallback Callback, void* Context);
//***************************************************************************//
//! \brief Sets up that a user-supplied function ("callback function") is called with each event of type "job_loaded_enough".
DLLAPI uint32_t slsc_cfg_register_callback_job_loaded_enough(size_t Handle, slsc_JobCallback Callback, void* Context);
//***************************************************************************//
//! \brief Sets up that a user-supplied function ("callback function") is called with each event of type "job_progress_planned".
DLLAPI uint32_t slsc_cfg_register_callback_job_progress_planned(size_t Handle, slsc_ExecTimeCallback Callback, void* Context);
//***************************************************************************//
//! \brief Sets up that a user-supplied function ("callback function") is called with each event of type "job_start_planned".
DLLAPI uint32_t slsc_cfg_register_callback_job_start_planned(size_t Handle, slsc_JobCallback Callback, void* Context);
//***************************************************************************//
//! \brief Destroys the specified (by the Handle) syncAXIS control instance and creates it again (maintaining the current configuration). In the process, the Handle value remains unchanged.
DLLAPI uint32_t slsc_cfg_reinitialize(size_t Handle);
//***************************************************************************//
//! \brief Destroys the specified (by the Handle) syncAXIS control instance and creates it again (by using the specified configuration file).In the process, the Handle value remains unchanged.
DLLAPI uint32_t slsc_cfg_reinitialize_from_file(size_t Handle, const char* XmlConfigFileName);
//***************************************************************************//
//! \brief The specified syncAXIS control instance temporarily releases the positioning stage. Then, it can be controlled externally(for example, by a non - syncAXIS control - based user program).
//! \note Deprecated - use slsc_ctrl_unfollow instead.
DLLAPI uint32_t slsc_cfg_release_stage(size_t Handle);
//***************************************************************************//
//! \brief Function for specifying the target positioning stage("positioning stage change").
DLLAPI uint32_t slsc_cfg_select_stage(size_t Handle, slsc_Stage Stage, uint32_t CorrectionFileIndex);
//***************************************************************************//
//! \brief Auxiliary function. Is obsolute use slsc_cfg_select_stage.
DLLAPI uint32_t slsc_cfg_select_stage_axis(size_t Handle, uint32_t StageAxisX, uint32_t StageAxisY, uint32_t SlecEtherCATNodeID, uint32_t DriveEtherCATNodeID, uint32_t CorrectionFileIndex);
//***************************************************************************//
//! \brief As of the next slsc_list_begin: switch on/off the "Contour-dependent speed calculation".
DLLAPI uint32_t slsc_cfg_set_contour_dependent_speed_control_2d(size_t Handle, int32_t Direction, double SpotRadius);
//***************************************************************************//
//! \brief Changes the jump speed as of the next list_begin.
DLLAPI uint32_t slsc_cfg_set_jump_speed(size_t Handle, double JumpSpeed);
//***************************************************************************//
//! \brief Sets up the return behavior of the Job functions (list_).
DLLAPI uint32_t slsc_cfg_set_list_handling_mode(size_t Handle, slsc_ListHandlingMode Mode, bool(*Predicate)(uint32_t));
//***************************************************************************//
//! \brief Sets up the return behavior of the Job functions (list_).
DLLAPI uint32_t slsc_cfg_set_list_handling_mode_with_context(size_t Handle, slsc_ListHandlingMode Mode, bool(*Predicate)(uint32_t, void*), void* Context);
//***************************************************************************//
//! \brief changes the mark speed in the trajectory layer with next list_start_job command (global configuration change, valid from the next job on for all jobs)
DLLAPI uint32_t slsc_cfg_set_mark_speed(size_t Handle, double MarkSpeed);
//***************************************************************************//
//! \brief changes the bandwidth of the filter for trajectory decomposition with next list_start_job command (global configuration change, valid from the next job on for all jobs)
DLLAPI uint32_t slsc_cfg_set_bandwidth(size_t Handle, double FilterBandwidth);
//***************************************************************************//
//! \brief changes the selection of DynamicReductionFunction that is to be used amongst those provided in the configuration xml.
DLLAPI uint32_t slsc_cfg_select_heuristic(size_t Handle, size_t HeuristicIndex);
//***************************************************************************//
//! \brief Changes target point coordinates according to a transformation matrix and an offset value.
DLLAPI uint32_t slsc_cfg_set_matrix_and_offset(size_t Handle, const double* Matrix, const double* Offset);
//***************************************************************************//
//! \brief Sets the operation mode of the syncAXIS-DLL instance.
DLLAPI uint32_t slsc_cfg_set_mode(size_t Handle, slsc_OperationMode Mode);
//***************************************************************************//
//! \brief Applies a transformation to the basic transformation for a specific scanner defined in xml.
DLLAPI uint32_t slsc_cfg_set_part_displacement(size_t Handle, slsc_ScanDevice ScanDevice, const double* Matrix, const double* Offset);
//***************************************************************************//
//! \brief Changes target point coordinates by an angle and an offset value as of the next slsc_list_begin.
DLLAPI uint32_t slsc_cfg_set_rot_and_offset_2d(size_t Handle, double Angle, const double* Offset);
//***************************************************************************//
//! \brief Sets the trajectory planning configuration for the currently running syncAXIS-DLL instance.
DLLAPI uint32_t slsc_cfg_set_trajectory_config(size_t Handle, const slsc_TrajectoryConfig* TrajConfig);
//***************************************************************************//
//! \brief Changes the field limits of the scanner.
DLLAPI uint32_t slsc_cfg_set_field_limits_scan_device(size_t Handle, const double* FieldLimitsMin, const double* FieldLimitsMax);
//***************************************************************************//
//! \brief Retrieves the field limits of the scanner.
DLLAPI uint32_t slsc_cfg_get_field_limits_scan_device(size_t Handle, double* FieldLimitsMin, double* FieldLimitsMax);
//***************************************************************************//
//! \brief Changes the field limits of the stage.
DLLAPI uint32_t slsc_cfg_set_field_limits_stage(size_t Handle, slsc_Stage Stage, const double* FieldLimitsMin, const double* FieldLimitsMax);
//***************************************************************************//
//! \brief Retrieves the field limits of the stage.
DLLAPI uint32_t slsc_cfg_get_field_limits_stage(size_t Handle, slsc_Stage Stage, double* FieldLimitsMin, double* FieldLimitsMax);
//***************************************************************************//
//! \brief Changes the dynamic limits of the scanner.
DLLAPI uint32_t slsc_cfg_set_dynamic_limits_scan_device(size_t Handle, double AngularVel, double AngularAcc, double AngularJerk);
//***************************************************************************//
//! \brief Retrieves the dynamic limits of the scanner.
DLLAPI uint32_t slsc_cfg_get_dynamic_limits_scan_device(size_t Handle, double* AngularVel, double* AngularAcc, double* AngularJerk);
//***************************************************************************//
//! \brief Retrieves the jump calculation dynamics of the scanner.
DLLAPI uint32_t slsc_cfg_get_calculation_dynamics_jump_scan_device(size_t Handle, double* JumpAngularAcc, double* JumpAngularJerk);
//***************************************************************************//
//! \brief Retrieves the mark calculation dynamics of the scanner.
DLLAPI uint32_t slsc_cfg_get_calculation_dynamics_mark_scan_device(size_t Handle, double* MarkAngularAcc, double* MarkAngularJerk);
//***************************************************************************//
//! \brief Changes the jump calculation dynamics of the scanner.
DLLAPI uint32_t slsc_cfg_set_calculation_dynamics_jump_scan_device(size_t Handle, double JumpAngularAcc, double JumpAngularJerk);
//***************************************************************************//
//! \brief Changes the mark calculation dynamics of the scanner.
DLLAPI uint32_t slsc_cfg_set_calculation_dynamics_mark_scan_device(size_t Handle, double MarkAngularAcc, double MarkAngularJerk);
//***************************************************************************//
//! \brief Changes the dynamic limits of the stage.
DLLAPI uint32_t slsc_cfg_set_dynamic_limits_stage(size_t Handle, slsc_Stage Stage, double StageVel, double StageAcc, double StageJerk);
//***************************************************************************//
//! \brief Retrieves the dynamic limits of the stage.
DLLAPI uint32_t slsc_cfg_get_dynamic_limits_stage(size_t Handle, slsc_Stage Stage, double* StageVel, double* StageAcc, double* StageJerk);
//***************************************************************************//
//! \brief Changes the calculation dynamics of the stage.
DLLAPI uint32_t slsc_cfg_set_calculation_dynamics_stage(size_t Handle, slsc_Stage Stage, double StageVel, double StageAcc, double StageJerk);
//***************************************************************************//
//! \brief Retrieves the calculation dynamics of the stage.
DLLAPI uint32_t slsc_cfg_get_calculation_dynamics_stage(size_t Handle, slsc_Stage Stage, double* StageVel, double* StageAcc, double* StageJerk);
//***************************************************************************//
//! \brief Changes the dynamic violation reaction.
DLLAPI uint32_t slsc_cfg_set_dynamic_violation_reaction(size_t Handle, slsc_DynamicViolationReaction DynamicViolationReaction);
//***************************************************************************//
//! \brief Retrieves the dynamic violation reaction.
DLLAPI uint32_t slsc_cfg_get_dynamic_violation_reaction(size_t Handle, slsc_DynamicViolationReaction* DynamicViolationReaction);
//***************************************************************************//
//! \brief Changes the dynamic monitoring level of the scanner.
DLLAPI uint32_t slsc_cfg_set_scan_device_dynamic_monitoring_level(size_t Handle, slsc_DynamicMonitoringLevel DynamicMonitoringLevel);
//***************************************************************************//
//! \brief Retrieves the dynamic monitoring level of the scanner.
DLLAPI uint32_t slsc_cfg_get_scan_device_dynamic_monitoring_level(size_t Handle, slsc_DynamicMonitoringLevel* DynamicMonitoringLevel);
//***************************************************************************//
//! \brief Changes the dynamic monitoring level of the stage.
DLLAPI uint32_t slsc_cfg_set_stage_dynamic_monitoring_level(size_t Handle, slsc_DynamicMonitoringLevel DynamicMonitoringLevel);
//***************************************************************************//
//! \brief Retrieves the dynamic monitoring level of the stage.
DLLAPI uint32_t slsc_cfg_get_stage_dynamic_monitoring_level(size_t Handle, slsc_DynamicMonitoringLevel* DynamicMonitoringLevel);
//***************************************************************************//
//! \brief Changes whether the instance is in simulation mode or hardware mode.
DLLAPI uint32_t slsc_cfg_set_simulation_setting(size_t Handle, slsc_SimulationSetting SimulationSetting);
//***************************************************************************//
//! \brief Retrieves whether the instance is in simulation mode or hardware mode.
DLLAPI uint32_t slsc_cfg_get_simulation_setting(size_t Handle, slsc_SimulationSetting* SimulationSetting);

//***************************************************************************//
// ctrl_* control functions
//***************************************************************************//

//***************************************************************************//
//! \brief Provided the RTC6 board is in "laser active" operation mode: disables the laser control signals LASERON, LASER1 and LASER2 at the RTC6 board.
DLLAPI uint32_t slsc_ctrl_disable_laser(size_t Handle);
//***************************************************************************//
//! \brief Provided the RTC6 board is in "laser active" operation mode: enables the laser control signals LASERON, LASER1 and LASER2 at the RTC6 board.
DLLAPI uint32_t slsc_ctrl_enable_laser(size_t Handle);
//***************************************************************************//
//! \brief Sets only the stage to follow
DLLAPI uint32_t slsc_ctrl_follow(size_t Handle);
//***************************************************************************//
//! \brief Returns additional information on an error that occurred(error number  ErrorNr).
DLLAPI uint32_t slsc_ctrl_get_error(size_t Handle, size_t ErrorNr, uint64_t* ErrorCode, char* ErrorMsg, size_t MsgBufSize);
//***************************************************************************//
//! \brief Returns the number of present errors.
DLLAPI uint32_t slsc_ctrl_get_error_count(size_t Handle, size_t* ErrorCount);
//***************************************************************************//
//! \brief Returns the execution state of the RTC6 board(from the perspective of the syncAXIS-DLL instance).
DLLAPI uint32_t slsc_ctrl_get_exec_state(size_t Handle, slsc_ExecState* State);
//***************************************************************************//
//! \brief Returns the current value of a free variable.
DLLAPI uint32_t slsc_ctrl_get_free_variable(size_t Handle, uint32_t Number, uint32_t* Value);
//***************************************************************************//
//! \brief Returns - for a specified Job-ID - the value of a Job characteristic ("Key", see enum slsc_JobCharacteristic) which has been calculated by the trajectory planning.
DLLAPI uint32_t slsc_ctrl_get_job_characteristic(size_t Handle, size_t JobID, slsc_JobCharacteristic Key, double* Value);
//***************************************************************************//
//! \brief Returns the present value of the specified signals for the specified head.
DLLAPI uint32_t slsc_ctrl_get_scan_device_position(size_t Handle, slsc_ScanDevice ScanDevice, slsc_PositionType Type, double* Position);
//***************************************************************************//
//! \brief Returns - for a specified Job-ID - the name of the simulation output file.
DLLAPI uint32_t slsc_ctrl_get_syncaxis_simulation_filename(size_t Handle, size_t JobID, char* SimulationFileName, size_t FileNameBufSize);
//***************************************************************************//
//! \brief Returns - for a specified Job-ID, and a specified ScanDevice - the name of the simulation output file.
//! \note Deprecated - filename is no longer dependent on ScanDevice, use slsc_ctrl_get_syncaxis_simulation_filename instead!
DLLAPI uint32_t slsc_ctrl_get_simulation_filename(size_t Handle, size_t JobID, slsc_ScanDevice ScanDevice, char* SimulationFileName, size_t FileNameBufSize);
//***************************************************************************//
//! \brief Returns the present value of the specified signals for the stage.
DLLAPI uint32_t slsc_ctrl_get_stage_position(size_t Handle, slsc_PositionType Type, double* Position);
//***************************************************************************//
//! \brief Returns the present value of the specified signals at the specified axis. Don't use this for positions anymore, use get_*_position
DLLAPI uint32_t slsc_ctrl_get_value(size_t Handle, size_t AxisIndex, slsc_MeasurementSignal Signal, double* Value);
//***************************************************************************//
//! \brief Checks whether the syncAXIS-DLL Input buffer is full (and therefore, cannot accept an additional Job function(list_) at the moment).
DLLAPI uint32_t slsc_ctrl_is_list_input_buffer_full(size_t Handle, bool* Flag);
//***************************************************************************//
//! \brief Only in mode "Manual Positioning": Switches the laser off immediately.
DLLAPI uint32_t slsc_ctrl_laser_signal_off(size_t Handle);
//***************************************************************************//
//! \brief Only in mode "Manual Positioning": Switches the laser on immediately.
DLLAPI uint32_t slsc_ctrl_laser_signal_on(size_t Handle);
//***************************************************************************//
//! \brief Only in mode "Manual Positioning": Moves the galvanometer scanners to the specified position with jump speed (starting from the current position). This function moves all scan devices (to separate positions).
DLLAPI uint32_t slsc_ctrl_move_scanner_abs(size_t Handle, const double* Position);
//***************************************************************************//
//! \brief Only in mode "Manual Positioning": Moves the positioning stage to the specified position with dynamics (acceleration and jerk) set by the ACS API (starting from the current position).
DLLAPI uint32_t slsc_ctrl_move_stage_abs(size_t Handle, const double* Position, double Speed, double Timeout);
//***************************************************************************//
//! \brief Immediately transfers a correction file to the RTC6.
DLLAPI uint32_t slsc_ctrl_refresh_correction_file(size_t Handle, uint32_t CorrectionFileIndex);
//***************************************************************************//
//! \brief To specify a correction file, which is to be used immediately.
DLLAPI uint32_t slsc_ctrl_select_correction_file(size_t Handle, uint32_t CorrectionFileIndex);
//***************************************************************************//
//! \brief Writes a 32-bit output Value to a free variable. With eight free variables at disposal, only the three least significant bits of the passed Number are evaluated.
DLLAPI uint32_t slsc_ctrl_set_free_variable(size_t Handle, uint32_t Number, uint32_t Value);
//***************************************************************************//
//! \brief Defines the output period and the pulse lengths for the laser signals LASER1 and LASER2 for "laser active" operation of the RTC6.
DLLAPI uint32_t slsc_ctrl_set_laser_pulses(size_t Handle, double HalfPeriod, double PulseLength);
//***************************************************************************//
//! \brief Tries to start the execution of a job by the RTC6 board.
DLLAPI uint32_t slsc_ctrl_start_execution(size_t Handle);
//***************************************************************************//
//! \brief Emergency-stops the execution of the current job on the RTC6 board.
DLLAPI uint32_t slsc_ctrl_stop(size_t Handle);
//***************************************************************************//
//! \brief Stops the execution of the current job via a controlled home jump.
DLLAPI uint32_t slsc_ctrl_stop_controlled(size_t Handle);
//***************************************************************************//
//! \brief Sets only the stage to unfollow
DLLAPI uint32_t slsc_ctrl_unfollow(size_t Handle);
//***************************************************************************//
//! \brief Writes a 12-bit output value to the 12-Bit-analog output port ANALOG OUT1 or ANALOG OUT2 of the RTC6. (This is only valid, until it is overwritten. This change is not saved in the configuration!)
DLLAPI uint32_t slsc_ctrl_write_analog_x(size_t Handle, slsc_AnalogOutput Channel, double Value);
//***************************************************************************//
//! \brief Writes a 16-bit output value to the 16-bit digital output port DIGITAL OUT 0...DIGITAL OUT 15 of the RTC6. (This is only valid, until it is overwritten. This change is not saved in the configuration!)
DLLAPI uint32_t slsc_ctrl_write_digital_out(size_t Handle, uint16_t Value);
//***************************************************************************//
//! \brief Writes only those bits of the Value to the 16-bit digital output port of the RTC6, which are specified in the Mask parameter. (This is only valid, until it is overwritten. This change is not saved in the configuration!)
DLLAPI uint32_t slsc_ctrl_write_digital_out_mask(size_t Handle, uint16_t Value, uint16_t Mask);

//***************************************************************************//
// list_* job commands
//***************************************************************************//

//***************************************************************************//
//! \brief Defines the beginning of a job. Is 1 of 2 mandatory structure elements of a job.
DLLAPI uint32_t slsc_list_begin(size_t Handle, size_t* JobID);
//***************************************************************************//
//! \brief Defines the beginning of a job. Is 1 of 2 mandatory structure elements of a job.
DLLAPI uint32_t slsc_list_begin_absolute(size_t Handle, size_t* JobID, const double* StartPosition);
//***************************************************************************//
//! \brief Defines the beginning of a job. Is 1 of 2 mandatory structure elements of a job. Like slsc_list_begin_absolute, but additionally the planned trajectory is recorded.
DLLAPI uint32_t slsc_list_begin_module(size_t Handle, size_t* JobID, const double* StartPosition, const char* ModuleFileName);
//***************************************************************************//
//! \brief Defines the beginning of a job. Is 1 of 2 mandatory structure elements of a job. Difference to slsc_list_begin: For ScannerAndStage and StageOnly applies the stage position (the stage would have at the start of the execution of this job) as an offset for this job (and this job only!)
DLLAPI uint32_t slsc_list_begin_relative(size_t Handle, size_t* JobID);
//***************************************************************************//
//! \brief Defines the end of a job. Is 1 of 2 mandatory structure elements of a job.
DLLAPI uint32_t slsc_list_end(size_t Handle);
//***************************************************************************//
//! \brief Defines a jump by absolute coordinate values.
DLLAPI uint32_t slsc_list_jump_abs(size_t Handle, const double* Target);
//***************************************************************************//
//! \brief Similar to slsc_list_jump_abs, but also defines a minimal duration for the jump.
DLLAPI uint32_t slsc_list_jump_abs_min_time(size_t Handle, const double* Target, double MinimalJumpTime);
//***************************************************************************//
//! \brief As slsc_list_jump_abs. But offers the argument ParaTarget additionally, by which a "ramp" is defined(in the working field, the value/s of one/two "ActiveChannel" is/are varied linearly).
DLLAPI uint32_t slsc_list_para_jump_abs(size_t Handle, const double* Target, const double* ParaTarget);
//***************************************************************************//
//! \brief Similar to slsc_list_para_jump_abs, but also defines a minimal duration for the jump.
DLLAPI uint32_t slsc_list_para_jump_abs_min_time(size_t Handle, const double* Target, double MinimalJumpTime, const double* ParaTarget);
//***************************************************************************//
//! \brief Defines a mark vector by absolute coordinate values.
DLLAPI uint32_t slsc_list_mark_abs(size_t Handle, const double* Target);
//***************************************************************************//
//! \brief As slsc_list_mark_abs. But offers the argument ParaTarget additionally, by which a "ramp" is defined(in the working field, the value/s of one/two "ActiveChannel" is/are varied linearly).
DLLAPI uint32_t slsc_list_para_mark_abs(size_t Handle, const double* Target, const double* ParaTarget);
//***************************************************************************//
//! \brief As slsc_list_mark_abs. But offers the argument MultiParaTarget additionally, by which (per "ActiveChannel") a "ramp" consisting of several sections is defined.
DLLAPI uint32_t slsc_list_multi_para_mark_abs(size_t Handle, const double* Target, const slsc_MultiParaTarget* MultiParaTarget);
//***************************************************************************//
//! \brief Defines a to-be-marked circular arc (not: elliptical arc) by absolute coordinate values.
DLLAPI uint32_t slsc_list_arc_abs(size_t Handle, const double* Mid, const double* Target);
//***************************************************************************//
//! \brief As slsc_list_arc_abs. But offers the argument ParaTarget additionally, by which a "ramp" is defined(in the working field, the value/s of one/two "ActiveChannel" is/are varied linearly).
DLLAPI uint32_t slsc_list_para_arc_abs(size_t Handle, const double* Mid, const double* Target, const double* ParaTarget);
//***************************************************************************//
//! \brief As slsc_list_arc_abs. But offers the argument MultiParaTarget additionally, by which (per "ActiveChannel") a "ramp" consisting of several sections is defined.
DLLAPI uint32_t slsc_list_multi_para_arc_abs(size_t Handle, const double* Mid, const double* Target, const slsc_MultiParaTarget* MultiParaTarget);
//***************************************************************************//
//! \brief Defines a circle (not: ellipse) by the absolute coordinate value of the circle center. The parameter Angle determines the marking direction as well as the number of rotations(for example, 3, 25 * 2 \pi).
DLLAPI uint32_t slsc_list_circle_2d_abs(size_t Handle, const double* Center, double Angle);
//***************************************************************************//
//! \brief As slsc_list_circle_2d_abs. But offers the argument  ParaTarget  additionally, by which a "ramp" is defined(in the working field, the value/s of one/two "ActiveChannel" is/are varied linearly).
DLLAPI uint32_t slsc_list_para_circle_2d_abs(size_t Handle, const double* Center, double Angle, const double* ParaTarget);
//***************************************************************************//
//! \brief As slsc_list_circle_2d_abs. But offers the argument MultiParaTarget additionally, by which(per "ActiveChannel") a "ramp" consisting of several sections is defined.
DLLAPI uint32_t slsc_list_multi_para_circle_2d_abs(size_t Handle, const double* Center, double Angle, const slsc_MultiParaTarget* MultiParaTarget);
//***************************************************************************//
//! \brief Defines a mark vector by absolute coordinate values with an array of switch postions where the laser is to be altenatingly turned on and off.
DLLAPI uint32_t slsc_list_dashed_mark_abs(size_t Handle, const double* Target, size_t NSwitches, const double* LaserSwitches);
//***************************************************************************//
//! \brief As slsc_list_dashed_mark_abs. But offers the argument ParaTarget additionally, by which a "ramp" is defined(in the working field, the value/s of one/two "ActiveChannel" is/are varied linearly).
DLLAPI uint32_t slsc_list_para_dashed_mark_abs(size_t Handle, const double* Target, size_t NSwitches, const double* LaserSwitches, const double* ParaTarget);
//***************************************************************************//
//! \brief As slsc_list_dashed_mark_abs. But offers the argument MultiParaTarget additionally, by which (per "ActiveChannel") a "ramp" consisting of several sections is defined.
DLLAPI uint32_t slsc_list_multi_para_dashed_mark_abs(size_t Handle, const double* Target, size_t NSwitches, const double* LaserSwitches, const slsc_MultiParaTarget* MultiParaTarget);
//***************************************************************************//
//! \brief Defines a to-be-marked circular arc (not: elliptical arc) by absolute coordinate values with an array of switch postions where the laser is to be altenatingly turned on and off.
DLLAPI uint32_t slsc_list_dashed_arc_abs(size_t Handle, const double* Mid, const double* Target, size_t NSwitches, const double* LaserSwitches);
//***************************************************************************//
//! \brief As slsc_list_dashed_arc_abs. But offers the argument ParaTarget additionally, by which a "ramp" is defined(in the working field, the value/s of one/two "ActiveChannel" is/are varied linearly).
DLLAPI uint32_t slsc_list_para_dashed_arc_abs(size_t Handle, const double* Mid, const double* Target, size_t NSwitches, const double* LaserSwitches, const double* ParaTarget);
//***************************************************************************//
//! \brief As slsc_list_dashed_arc_abs. But offers the argument MultiParaTarget additionally, by which (per "ActiveChannel") a "ramp" consisting of several sections is defined.
DLLAPI uint32_t slsc_list_multi_para_dashed_arc_abs(size_t Handle, const double* Mid, const double* Target, size_t NSwitches, const double* LaserSwitches, const slsc_MultiParaTarget* MultiParaTarget);
//***************************************************************************//
//! \brief Defines a circle (not: ellipse) by the absolute coordinate value of the circle center with an array of switch postions where the laser is to be altenatingly turned on and off. The parameter Angle determines the marking direction as well as the number of rotations(for example, 3, 25 * 2 \pi).
DLLAPI uint32_t slsc_list_dashed_circle_2d_abs(size_t Handle, const double* Center, double Angle, size_t NSwitches, const double* LaserSwitches);
//***************************************************************************//
//! \brief As slsc_list_dashed_circle_2d_abs. But offers the argument  ParaTarget  additionally, by which a "ramp" is defined(in the working field, the value/s of one/two "ActiveChannel" is/are varied linearly).
DLLAPI uint32_t slsc_list_para_dashed_circle_2d_abs(size_t Handle, const double* Center, double Angle, size_t NSwitches, const double* LaserSwitches, const double* ParaTarget);
//***************************************************************************//
//! \brief As slsc_list_dashed_circle_2d_abs. But offers the argument MultiParaTarget additionally, by which(per "ActiveChannel") a "ramp" consisting of several sections is defined.
DLLAPI uint32_t slsc_list_multi_para_dashed_circle_2d_abs(size_t Handle, const double* Center, double Angle, size_t NSwitches, const double* LaserSwitches, const slsc_MultiParaTarget* MultiParaTarget);
//***************************************************************************//
//! \brief Switches the processing of the arguments ParaTarget (of slsc_list_para* functions) and MultiParaTarget(of slsc_list_multi_para* functions) off.
DLLAPI uint32_t slsc_list_para_disable(size_t Handle);
//***************************************************************************//
//! \brief Switches the processing of the arguments ParaTarget (of slsc_list_para* functions) and MultiParaTarget(of slsc_list_multi_para* functions) on.
DLLAPI uint32_t slsc_list_para_enable(size_t Handle, const double* ParaTargetDefault);
//***************************************************************************//
//! \brief As slsc_list_playback_module. Additionally the recorded "ActiveChannel" variation factors are used if enabled via slsc_list_para_enable.
DLLAPI uint32_t slsc_list_para_playback_module(size_t Handle, const char* ModuleFileName);
//***************************************************************************//
//! \brief Playbacks a recorded job.
DLLAPI uint32_t slsc_list_playback_module(size_t Handle, const char* ModuleFileName);
//***************************************************************************//
//! \brief Changes the ApproxBlendLimit value. This change applies to all following Job functions (list_)until the end of the job.
DLLAPI uint32_t slsc_list_set_approx_blend_limit(size_t Handle, double ApproxBlendLimit);
//***************************************************************************//
//! \brief Switches on/off the "Contour-dependent speed calculation".
DLLAPI uint32_t slsc_list_set_contour_dependent_speed_control_2d(size_t Handle, int32_t Direction, double SpotRadius);
//***************************************************************************//
//! \brief Writes a 32-bit output Value to a free variable. With eight free variables at disposal, only the three least significant bits of the passed Number are evaluated.
DLLAPI uint32_t slsc_list_set_free_variable(size_t Handle, uint32_t Number, uint32_t Value, double TimeDelay);
//***************************************************************************//
//! \brief Changes the jump speed. This change applies to all following Job functions (list_) until the end of the job.
DLLAPI uint32_t slsc_list_set_jump_speed(size_t Handle, double JumpSpeed);
//***************************************************************************//
//! \brief Delays the output of the LASERON signal by an amount that is equivalent to a specified PathLength. This change applies to all following Job functions (list_) until the end of the job.
DLLAPI uint32_t slsc_list_set_laser_on_move(size_t Handle, double PathLength);
//***************************************************************************//
//! \brief Defines the output period and the pulse lengths for the laser signals LASER1 and LASER2 for "laser active" operation of the RTC6.
DLLAPI uint32_t slsc_list_set_laser_pulses(size_t Handle, double HalfPeriod, double PulseLength, double TimeDelay);
//***************************************************************************//
//! \brief Changes the mark speed. This change applies to all following Job functions (list_) until the end of the job.
DLLAPI uint32_t slsc_list_set_mark_speed(size_t Handle, double MarkSpeed);
//***************************************************************************//
//! \brief Changes the minimal mark speed. This change applies to all following Job functions (list_) until the end of the job.
DLLAPI uint32_t slsc_list_set_min_mark_speed(size_t Handle, double MinimalMarkSpeed);
//***************************************************************************//
//! \brief Changes the calculation dynamics (acceleration and jerk) of the scan head for jumps. This change applies to all following Job functions (list_) until the end of the job.
DLLAPI uint32_t slsc_list_set_calculation_dynamics_jump_scan_device(size_t Handle, double JumpAngularAcc, double JumpAngularJerk);
//***************************************************************************//
//! \brief Changes the calculation dynamics (acceleration and jerk) of the scan head for markings. This change applies to all following Job functions (list_) until the end of the job.
DLLAPI uint32_t slsc_list_set_calculation_dynamics_mark_scan_device(size_t Handle, double MarkAngularAcc, double MarkAngularJerk);
//***************************************************************************//
//! \brief Changes target point coordinates according to a transformation matrix and an offset value.
DLLAPI uint32_t slsc_list_set_matrix_and_offset(size_t Handle, const double* Matrix, const double* Offset);
//***************************************************************************//
//! \brief Changes target point coordinates (see below) by an angle and an offset value.
DLLAPI uint32_t slsc_list_set_rot_and_offset_2d(size_t Handle, double Angle, const double* Offset);
//***************************************************************************//
//! \brief Provided SpotDistance is chosen as an ActiveChannel: suppresses spotdistance control.
DLLAPI uint32_t slsc_list_suppress_spotdistance_control(size_t Handle, double TimeDelay);
//***************************************************************************//
//! \brief Provided SpotDistance is chosen as an ActiveChannel: resets spotdistance control.
DLLAPI uint32_t slsc_list_unsuppress_spotdistance_control(size_t Handle, double TimeDelay);
//***************************************************************************//
//! \brief Defines a waiting time with which the laser spot is to wait at the last defined target point with the laser switched off.
DLLAPI uint32_t slsc_list_wait_with_laser_off(size_t Handle, double Time);
//***************************************************************************//
//! \brief Defines a waiting time with which the laser spot is to wait at the last defined target point with the laser switched on.
DLLAPI uint32_t slsc_list_wait_with_laser_on(size_t Handle, double Time);
//***************************************************************************//
//! \brief Writes a 12-bit output value to the 12-Bit-analog output port ANALOG OUT1 or ANALOG OUT2 of the RTC6.
DLLAPI uint32_t slsc_list_write_analog_x(size_t Handle, slsc_AnalogOutput Channel, double Value, double TimeDelay);
//***************************************************************************//
//! \brief Writes a 16-bit output value to the 16-bit digital output port DIGITAL OUT 0...DIGITAL OUT 15 of the RTC6.
DLLAPI uint32_t slsc_list_write_digital_out(size_t Handle, uint16_t Value, double TimeDelay);
//***************************************************************************//
//! \brief Writes only those bits of the Value to the 16-bit digital output port of the RTC6, which are specified in the Mask parameter.
DLLAPI uint32_t slsc_list_write_digital_out_mask(size_t Handle, uint16_t Value, uint16_t Mask, double TimeDelay);

//***************************************************************************//
// Additional syncAXIS related utility commands.
//***************************************************************************//

//***************************************************************************//
//! \brief Resets all found PCIe boards with the specified program file path.
//! Be careful, this will influence all running instances!
DLLAPI uint32_t slsc_util_reset_pcie(const char* PathToProgramFile);



#ifdef __cplusplus
};
#endif

#endif
