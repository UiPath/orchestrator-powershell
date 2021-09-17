﻿using System;
using System.Collections;
using System.ComponentModel;
using UiPath.PowerShell.Util;
using MachineDto20194 = UiPath.Web.Client20194.Models.MachineDto;
using MachineDto20204 = UiPath.Web.Client20204.Models.MachineDto;
using MachineDtoType = UiPath.Web.Client20194.Models.MachineDtoType;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class Machine
    {
        public long? Id { get; private set; }
        public string LicenseKey { get; private set; }
        public string Name { get; private set; }
        public int? NonProductionSlots { get; private set; }
        public string Type { get; private set; }
        public int? UnattendedSlots { get; private set; }
        public Hashtable RobotVersions { get; private set; }
        public int? TestAutomationSlots { get; private set; }
        public int? HeadlessSlots { get; private set; }

        internal static object FromDto<TDto>(TDto dto) => dto.To<Machine>();

        internal static MachineDto20204 ToDto20204(Machine machine) => machine.To<MachineDto20204>();

        internal MachineDto20194 ToDto20194(Machine machine) => new MachineDto20194
        {
            Id = Id,
            LicenseKey = LicenseKey,
            Name = Name,
            NonProductionSlots = NonProductionSlots,
            Type = (MachineDtoType)Enum.Parse(typeof(MachineDtoType), Type),
            UnattendedSlots = UnattendedSlots
        };
    }
}