﻿using SenssehatCompanion.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace SenssehatCompanion.Models
{
    public enum JoystickEvent
    {
        [StringValue("pressed")]
        Pressed = 'p',
        [StringValue("released")]
        Released = 'r',
        [StringValue("held")]
        Held = 'h'
    }
    public enum JoystickStatu
    {
        [StringValue("left")]
        Left = 'l',
        [StringValue("right")]
        Right = 'r',
        [StringValue("up")]
        Up = 'u',
        [StringValue("down")]
        Down = 'd',
        [StringValue("middle")]
        Middle = 'm',
    }
    public class Joystick
    {
        public JoystickEvent action { get; set; }
        public JoystickStatu direction { get; set; }
    }
}
