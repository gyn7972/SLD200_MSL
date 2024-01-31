#pragma once
/*****************************************************************************/
/*
 * Import header for syncAXIS version 1.6
 * 
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
#include <cstddef> //AXIVION Line Generic-LocalInclude : needed for size_t on linux plattforms

//***************************************************************************//
//! \brief This enum defines the choices for the return behavior of the Job functions(list_).
enum slsc_ListHandlingMode
{
    slsc_ListHandlingMode_ReturnAtOnce           = 0, //!< No handling.
    slsc_ListHandlingMode_RepeatWhileBufferFull  = 1, //!< It is tried to execute the Job function until the return value is no longer 0x 00 00 00 10(BUFFER_FULL).
    slsc_ListHandlingMode_RepeatWhilePredicate   = 2, //!< It is tried to execute the Job function until the return value of the Predicate function is no longer True.
};

//***************************************************************************//
//! \brief This enum defines the choices for the operation mode of the syncAXIS-DLL instance.
enum slsc_OperationMode
{
    slsc_OperationMode_ScannerOnly               = 0, //!< Only scan system, no stage.
    slsc_OperationMode_StageOnly                 = 1, //!< Only stage, no scan system.
    slsc_OperationMode_ScannerAndStage           = 2, //!< Both, scan system and stage.

};

//***************************************************************************//
//! \brief This enum defines the choices for the simulation setting of the syncAXIS-DLL instance.
enum slsc_SimulationSetting
{
    slsc_SimulationSetting_SimulationMode   = 0, //!< Instance will not attempt to connect to any hardware and merely simulate jobs on the PC.
    slsc_SimulationSetting_HardwareMode     = 1, //!< Instance will connect to all hardware and physically execute jobs.
};

//***************************************************************************//
//! \brief This enum defines the reaction of the program should a dynamic limit be violated
enum slsc_DynamicViolationReaction
{
    slsc_DynamicViolationReaction_WarningOnly       = 0, //!< Log a warning message.
    slsc_DynamicViolationReaction_AbortImmediately  = 1, //!< Immediately abort job execution.
    slsc_DynamicViolationReaction_StopAndReport     = 2, //!< End the job and do a controlled jump to home position.
};

//***************************************************************************//
//! \brief This enum defines the dynamic parameters which should be monitored
enum slsc_DynamicMonitoringLevel
{
    slsc_DynamicMonitoringLevel_Deactivated     = 0, //!< No dynamic monitoring
    slsc_DynamicMonitoringLevel_Position        = 1, //!< Monitor only position.
    slsc_DynamicMonitoringLevel_Velocity        = 2, //!< Additionally monitor velocity.
    slsc_DynamicMonitoringLevel_Acceleration    = 3, //!< Additionally monitor acceleration.
    slsc_DynamicMonitoringLevel_Jerk            = 4, //!< Additionally monitor jerk.
};

//***************************************************************************//
//! \brief This enum defines the choices for the execution status of the RTC6 board(from the perspective of the syncAXIS - DLL instance).
enum slsc_ExecState
{
    slsc_ExecState_Idle                          = 0, //!< The RTC6 board is idle.
    slsc_ExecState_ReadyForExecution             = 1, //!< The RTC6 board is ready to execute jobs.
    slsc_ExecState_Executing                     = 2, //!< The RTC6 board is executing a job.
    slsc_ExecState_NotInitOrError                = 3, //!< The RTC6 board is not initialized or an error is present.
};

//***************************************************************************//
//! \brief This enum defines the choices for the analog output ports.
enum slsc_AnalogOutput
{
    slsc_AnalogOutput_1                          = 0, //!< Analog output port 1.
    slsc_AnalogOutput_2                          = 1, //!< Analog output port 2.
};

//***************************************************************************//
//! \brief This enum defines the choices for the operation status of the syncAXIS-DLL instance.
enum slsc_OperationStatus
{
    slsc_OperationStatus_Green                    = 0, //!< The syncAXIS-DLL instance is running and no errors occurred.
    slsc_OperationStatus_Yellow                   = 1, //!< The syncAXIS-DLL instance is not yet running because the initialization is still in progress.
    slsc_OperationStatus_Red                      = 2, //!< The syncAXIS-DLL instance is not running and at least one error occurred.
};

//***************************************************************************//
//! \brief This enum defines the choices for the measurement signals.
enum slsc_MeasurementSignal
{
    slsc_MeasurementSignal_Status                = 0, //!< Deprecated, use slsc_PositionType
    slsc_MeasurementSignal_Sample                = 1, //!< Position value of the specified axis. Deprecated, use slsc_PositionType
    slsc_MeasurementSignal_AnalogOutput_1        = 2, //!< Value at the analog output port 1.
    slsc_MeasurementSignal_AnalogOutput_2        = 3, //!< Value at the analog output port 2.
    slsc_MeasurementSignal_Errors                = 4, //!< Reserved
};

//***************************************************************************//
//! \brief This enum defines the choices for the position types.
enum slsc_PositionType
{
    slsc_PositionType_Set = 0, //!< Soll-Position
    slsc_PositionType_Actual  = 1, //!< Ist-Position
};

//***************************************************************************//
//! \brief This enum defines the choices for the blend modes.
enum slsc_BlendModes
{
    slsc_BlendModes_Deactivated = 0,        //!< The syncAXIS-DLL never inserts a blend curve at any corner point.
    slsc_BlendModes_VariableBlending = 1,   //!< Blend curves to corner points are always calculated to have the greatest distance.
    slsc_BlendModes_MinimalBlending = 2,    //!< Blend curves to corner points are always calculated to have the smallest distance.
    slsc_BlendModes_FixedBlending = 3,      //!< Deprecated. Blend curves are relized via composite splines.
};

//***************************************************************************//
//! \brief This enum defines the choices for the spline mode.
enum slsc_SplineModes
{
    slsc_SplineModes_Deactivated = 0,      //!< No splines.
    slsc_SplineModes_Interpolating = 1,      //!< Interpolating splines are used.
    slsc_SplineModes_Approximating = 2,      //!< Approximating splines are used.
};

//! \brief This enum defines the scan device for wich a setting (e.g. transformation) should be applied to
enum slsc_ScanDevice
{
    slsc_ScanDevice_None = 0,
    slsc_ScanDevice1 = 1,
    slsc_ScanDevice2 = 2,
    slsc_ScanDevice3 = 3,
    slsc_ScanDevice4 = 4,

};

//! \brief This enum defines the stage which should be selected
enum slsc_Stage
{
    slsc_Stage_None = 0,
    slsc_Stage1 = 1,
    slsc_Stage2 = 2,
    slsc_Stage3 = 3,
    slsc_Stage4 = 4,
};

//! \brief This enum defines the choices for the Job characteristic ("Key").
enum slsc_JobCharacteristic
{
    slsc_JobCharacteristic_ScannerPosX           = 0,    //!< Max. absolute scan head position in X direction. In mm.
    slsc_JobCharacteristic_ScannerPosY           = 1,    //!< Max. absolute scan head position in Y direction. In mm.
    slsc_JobCharacteristic_StagePosX             = 2,    //!< Max. absolute stage position in X direction. In mm.
    slsc_JobCharacteristic_StagePosY             = 3,    //!< Max. absolute stage position in Y direction. In mm.
    slsc_JobCharacteristic_ScannerVelX           = 4,    //!< Max. absolute scan head velocity in X direction. In mm/s.
    slsc_JobCharacteristic_ScannerVelY           = 5,    //!< Max. absolute scan head velocity in Y direction. In mm/s.
    slsc_JobCharacteristic_StageVelX             = 6,    //!< Max. absolute stage velocity in X direction. In mm/s.
    slsc_JobCharacteristic_StageVelY             = 7,    //!< Max. absolute stage velocity in Y direction. In mm/s.
    slsc_JobCharacteristic_ScannerAccX           = 8,    //!< Max. absolute scan head acceleration in X direction. In mm/s^2.
    slsc_JobCharacteristic_ScannerAccY           = 9,    //!< Max. absolute scan head acceleration in Y direction. In mm/s^2.
    slsc_JobCharacteristic_StageAccX             = 10,   //!< Max. absolute stage acceleration in X direction. In mm/s^2.
    slsc_JobCharacteristic_StageAccY             = 11,   //!< Max. absolute stage acceleration in Y direction. In mm/s^2.
    slsc_JobCharacteristic_StageJerkX            = 12,   //!< Max. absolute stage jerk in X direction. In mm/s^3.
    slsc_JobCharacteristic_StageJerkY            = 13,   //!< Max. absolute stage jerk in Y direction. In mm/s^3.
    slsc_JobCharacteristic_MotionMicroSteps      = 14,   //!< Number of micro vectors that make up the job.

    slsc_JobCharacteristic_ScannerPosXLaserOn    = 30,   //!< Max. absolute scan head position in X direction with active laser. In mm.
    slsc_JobCharacteristic_ScannerPosYLaserOn    = 31,   //!< Max. absolute scan head position in Y direction with active laser. In mm.
    slsc_JobCharacteristic_StagePosXLaserOn      = 32,   //!< Max. absolute stage position in X direction with active laser. In mm.
    slsc_JobCharacteristic_StagePosYLaserOn      = 33,   //!< Max. absolute stage position in Y direction with active laser. In mm.
    slsc_JobCharacteristic_MinimalMarkSpeed      = 50,   //!< Minimum velocity reached by the laser spot during the job.
    slsc_JobCharacteristic_MaximalMarkSpeed      = 51,   //!< Maximum velocity reached by the laser spot during the job.

    slsc_JobCharacteristic_InsertedSkywritings   = 200,  //!< Number of transitions in the job where a skywriting had to be inserted.
    slsc_JobCharacteristic_ScannerPosXmax        = 251,  //!< Max. scan head position in X direction. In mm.
    slsc_JobCharacteristic_ScannerPosXmin        = 252,  //!< Min. scan head position in X direction. In mm.
    slsc_JobCharacteristic_ScannerPosYmax        = 253,  //!< Max. scan head position in Y direction. In mm.
    slsc_JobCharacteristic_ScannerPosYmin        = 254,  //!< Min. scan head position in Y direction. In mm.
    slsc_JobCharacteristic_ScannerPosXmaxLaserOn = 255,  //!< Max. scan head position in X direction with active laser. In mm.
    slsc_JobCharacteristic_ScannerPosXminLaserOn = 256,  //!< Min. scan head position in X direction with active laser. In mm.
    slsc_JobCharacteristic_ScannerPosYmaxLaserOn = 257,  //!< Max. scan head position in Y direction with active laser. In mm.
    slsc_JobCharacteristic_ScannerPosYminLaserOn = 258,  //!< Min. scan head position in Y direction with active laser. In mm.
    slsc_JobCharacteristic_StagePosXmax          = 259,  //!< Max. stage position in X direction. In mm.
    slsc_JobCharacteristic_StagePosXmin          = 260,  //!< Min. stage position in X direction. In mm.
    slsc_JobCharacteristic_StagePosYmax          = 261,  //!< Max. stage position in Y direction. In mm.
    slsc_JobCharacteristic_StagePosYmin          = 262,  //!< Min. stage position in Y direction. In mm.
    slsc_JobCharacteristic_StagePosXmaxLaserOn   = 263,  //!< Max. stage position in X direction with active laser. In mm.
    slsc_JobCharacteristic_StagePosXminLaserOn   = 264,  //!< Min. stage position in X direction with active laser. In mm.
    slsc_JobCharacteristic_StagePosYmaxLaserOn   = 265,  //!< Max. stage position in Y direction with active laser. In mm.
    slsc_JobCharacteristic_StagePosYminLaserOn   = 266,  //!< Min. stage position in Y direction with active laser. In mm.

};

//***************************************************************************//
//! \brief This structure defines one section of the "ramp" (which is defined by slsc_MultiParaTarget).
struct slsc_ParaSection
{
    double ds;          //!< Length of the section. 0 is also allowed (= step change). In mm.
    double ParaTarget;  //!< Factor lp at the end of ds. No unit.
};

//***************************************************************************//
//! \brief This structure defines a "ramp", which consists of several sections (these are defined with slsc_ParaSection).
struct slsc_MultiParaTarget
{
    slsc_ParaSection* Targets;  //!< Pointer to an array of variable size (which is specified in NumParaTargets).
    size_t NumParaTargets;      //!< Number of elements in Targets .
};

//***************************************************************************//
//! \brief Auxiliary data types
typedef void(*slsc_JobCallback)(size_t JobID, void* Context);
typedef void(*slsc_ExecTimeCallback)(size_t JobID, uint64_t Progress, double ExecTime, void* Context);

//***************************************************************************//
//! \brief This structure defines the basic settings for markings, for example, laser switching times.
struct slsc_MarkConfig
{
    double LaserPreTriggerTime;              //!< Time to trigger the laser signal in advance, if a mark segment is executed. Unit: s.
    double LaserSwitchOffsetTime;            //!< Time shift for the laser signals output. Unit: s.
    double LaserMinOffTime;                  //!< Minimum time interval with laser off signal inbetween two activations. Unit: s.
    double JumpSpeed;                        //!< Highest desired jump speed.
    double MarkSpeed;                        //!< Highest desired mark speed.
    double MinimalMarkSpeed;                 //!< Lowest desired mark speed.
};

//***************************************************************************//
//! \brief This structure defines the behavior of the blend curves and splines.
struct slsc_GeometryConfig
{
    double           MaxBlendRadius;                 //!< Maximum radius for blends between two vectors.
    double           ApproxBlendLimit;               //!< Maximum tolerable distance of the blend curve from the corner.
    slsc_BlendModes  BlendMode;                      //!< Blend mode to be applied.
    double           VectorResolution;               //!< All points with distances below this limit are treated as identical.
    bool             AutoCyclicGeometry;             //!< If "true": the syncAXIS-DLL interprets a spline as a "cyclic spline". Deprecated setting, it is recommended not to change this.
    double           SplineConversionLengthLimit;    //!< If the distance from last target point to target of a new list_mark_abs is smaller than the SplineConversionLengthLimit value, then the target point is added to a spline. Deprecated setting, it is recommended not to change this.
    slsc_SplineModes SplineMode;                     //!< Spline mode to be applied. Deprecated setting, it is recommended to leave in slsc_SplineModes_Deactivated.
};

//***************************************************************************//
//! \brief This structure defines the trajectory configuration.
struct slsc_TrajectoryConfig
{
    slsc_MarkConfig      MarkConfig;     //!< Configures the basic settings for markings.
    slsc_GeometryConfig  GeometryConfig; //!< Configures the behavior of blend curves and splines.
};

