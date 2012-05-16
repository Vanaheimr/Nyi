/*
 * Copyright (c) 2011-2012, Belectric ITS GmbH <opensource@belectric.com>
 * Autor: Achim 'ahzf' Friedland <achim.friedland@belectric.com>
 * This file is part of Vanaheimr Nyi <http://www.github.com/Vanaheimr/Nyi>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;

#endregion

namespace de.ahzf.Vanaheimr.Nyi.Chips.ADE7533
{

    public enum registers
    {
        WAVEFORM    = 0x01,
        AENERGY     = 0x02,
        RAENERGY    = 0x03,
        LAENERGY    = 0x04,
        VAENERGY    = 0x05,
        RVAENERGY   = 0x06,
        LVAENERGY   = 0x07,
        LVARENERGY  = 0x08,
        MODE        = 0x09,
        IRQEN       = 0x0A,
        STATUS      = 0x0B,
        RSTSTATUS   = 0x0C,
        CH1OS       = 0x0D,
        CH2OS       = 0x0E,
        GAIN        = 0x0F,
        PHCAL       = 0x10,
        APOS        = 0x11,
        WGAIN       = 0x12,
        WDIV        = 0x13,
        CFNUM       = 0x14,
        CFDEN       = 0x15,
        IRMS        = 0x16,
        VRMS        = 0x17,
        IRMSOS      = 0x18,
        VRMSOS      = 0x19,
        VAGAIN      = 0x1A,
        VADIV       = 0x1B,
        LINECYC     = 0x1C,
        ZXTOUT      = 0x1D,
        SAGCYC      = 0x1E,
        SAGLVL      = 0x1F,
        IPKLVL      = 0x20,
        VPKLVL      = 0x21,
        IPEAK       = 0x22,
        RSTIPEAK    = 0x23,
        VPEAK       = 0x24,
        RSTVPEAK    = 0x25,
        TEMP        = 0x26,
        PERIOD      = 0x27,
        TMODE       = 0x3D,
        CHKSUM      = 0x3E,
        DIEREV      = 0X3F
    }


    //bits
    [Flags]
    public enum mode_registers
    {
        DISHPF      =  0,
        DISLPF2     =  1, 
        DISCF       =  2,
        DISSAG      =  3,
        ASUSPEND    =  4,
        TEMPSEL     =  5,
        SWRST       =  6,
        CYCMODE     =  7,
        DISCH1      =  8,
        DISCH2      =  9,
        SWAP        = 10,
        DTRT1       = 11,
        DTRT0       = 12,
        WAVSEL1     = 13,
        WAVSEL0     = 14,
        POAM        = 15
    }


    public enum GAIN
    {
        GAIN_1      = 0x0,
        GAIN_2      = 0x1,
        GAIN_4      = 0x2,
        GAIN_8      = 0x3,
        GAIN_16     = 0x4
    }

    public enum INTEGRATOR
    {
        ON          = 1,
        OFF         = 0
    }


// Class Atributes
//#define CS 10                 // Chip Select ADE7753   
//#define WRITE 0x80
//#define CLKIN 4000000         //ADE7753 frec, max 4MHz
//#define METER_ID 42         //meter ID (used in xbee)

}
