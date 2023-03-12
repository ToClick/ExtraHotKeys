using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ExtraHotKeys
{


    public enum VKeys : short
    {
        [Description("LBTN")]
        LeftButton = 0x01,

        [Description("RBTN")]
        RightButton = 0x02,

        [Description("CANCEL")]
        Cancel = 0x03,

        [Description("BTNM")]
        MiddleButton = 0x04,

        [Description("EBTN")]
        ExtraButton1 = 0x05,

        [Description("RBTN")]
        ExtraButton2 = 0x06,

        [Description("BACK")]
        Back = 0x08,

        [Description("TAB")]
        Tab = 0x09,

        [Description("CLEAR")]
        Clear = 0x0C,

        [Description("RETURN")]
        Return = 0x0D,

        [Description("SHIFT")]
        Shift = 0x10,

        [Description("CONTROL")]
        Control = 0x11,

        [Description("MENU")]
        Menu = 0x12,

        [Description("PAUSE")]
        Pause = 0x13,

        [Description("CAPLOCK")]
        CapsLock = 0x14,

        [Description("KANA")]
        Kana = 0x15,

        [Description("HANGEUL")]
        Hangeul = 0x15,

        [Description("HANGUL")]
        Hangul = 0x15,

        [Description("JUNJA")]
        Junja = 0x17,

        [Description("FINAL")]
        Final = 0x18,

        [Description("HANJA")]
        Hanja = 0x19,

        [Description("KANJI")]
        Kanji = 0x19,

        [Description("ESC")]
        Escape = 0x1B,

        [Description("CON")]
        Convert = 0x1C,

        [Description("NCON")]
        NonConvert = 0x1D,

        [Description("ACCEPT")]
        Accept = 0x1E,

        [Description("MCH")]
        ModeChange = 0x1F,

        [Description("SPACE")]
        Space = 0x20,

        [Description("PRIOR")]
        Prior = 0x21,

        [Description("NEXT")]
        Next = 0x22,

        [Description("END")]
        End = 0x23,

        [Description("HOME")]
        Home = 0x24,

        [Description("LEFT")]
        Left = 0x25,

        [Description("UP")]
        Up = 0x26,

        [Description("RIGHT")]
        Right = 0x27,

        [Description("DOWN")]
        Down = 0x28,

        [Description("SELECT")]
        Select = 0x29,

        [Description("PRINT")]
        Print = 0x2A,

        [Description("EXECUTE")]
        Execute = 0x2B,

        [Description("SNAPSHOT")]
        Snapshot = 0x2C,

        [Description("INSERT")]
        Insert = 0x2D,

        [Description("DELETE")]
        Delete = 0x2E,

        [Description("HELP")]
        Help = 0x2F,

        [Description("0")]
        N0 = 0x30,

        [Description("1")]
        N1 = 0x31,

        [Description("2")]
        N2 = 0x32,

        [Description("3")]
        N3 = 0x33,

        [Description("4")]
        N4 = 0x34,

        [Description("5")]
        N5 = 0x35,

        [Description("6")]
        N6 = 0x36,

        [Description("7")]
        N7 = 0x37,

        [Description("8")]
        N8 = 0x38,

        [Description("9")]
        N9 = 0x39,

        [Description("A")]
        A = 0x41,

        [Description("B")]
        B = 0x42,

        [Description("C")]
        C = 0x43,

        [Description("D")]
        D = 0x44,

        [Description("E")]
        E = 0x45,

        [Description("F")]
        F = 0x46,

        [Description("G")]
        G = 0x47,

        [Description("H")]
        H = 0x48,

        [Description("I")]
        I = 0x49,

        [Description("J")]
        J = 0x4A,

        [Description("K")]
        K = 0x4B,

        [Description("L")]
        L = 0x4C,

        [Description("M")]
        M = 0x4D,

        [Description("N")]
        N = 0x4E,

        [Description("O")]
        O = 0x4F,

        [Description("P")]
        P = 0x50,

        [Description("Q")]
        Q = 0x51,

        [Description("R")]
        R = 0x52,

        [Description("S")]
        S = 0x53,

        [Description("T")]
        T = 0x54,

        [Description("U")]
        U = 0x55,

        [Description("V")]
        V = 0x56,

        [Description("W")]
        W = 0x57,

        [Description("X")]
        X = 0x58,

        [Description("Y")]
        Y = 0x59,

        [Description("Z")]
        Z = 0x5A,

        [Description("LWIN")]
        LeftWindows = 0x5B,

        [Description("RWIN")]
        RightWindows = 0x5C,

        [Description("APP")]
        Application = 0x5D,

        [Description("SLEEP")]
        Sleep = 0x5F,

        [Description("N0")]
        Numpad0 = 0x60,

        [Description("N1")]
        Numpad1 = 0x61,

        [Description("N2")]
        Numpad2 = 0x62,

        [Description("N3")]
        Numpad3 = 0x63,

        [Description("N4")]
        Numpad4 = 0x64,

        [Description("N5")]
        Numpad5 = 0x65,

        [Description("N6")]
        Numpad6 = 0x66,

        [Description("N7")]
        Numpad7 = 0x67,

        [Description("N8")]
        Numpad8 = 0x68,

        [Description("N9")]
        Numpad9 = 0x69,

        [Description("MUL")]
        Multiply = 0x6A,

        [Description("ADD")]
        Add = 0x6B,

        [Description("SEP")]
        Separator = 0x6C,

        [Description("SUB")]
        Subtract = 0x6D,

        [Description("DEC")]
        Decimal = 0x6E,

        [Description("DIV")]
        Divide = 0x6F,

        [Description("F1")]
        F1 = 0x70,

        [Description("F2")]
        F2 = 0x71,

        [Description("F3")]
        F3 = 0x72,

        [Description("F4")]
        F4 = 0x73,

        [Description("F5")]
        F5 = 0x74,

        [Description("F6")]
        F6 = 0x75,

        [Description("F7")]
        F7 = 0x76,

        [Description("F8")]
        F8 = 0x77,

        [Description("F9")]
        F9 = 0x78,

        [Description("F10")]
        F10 = 0x79,

        [Description("F11")]
        F11 = 0x7A,

        [Description("F12")]
        F12 = 0x7B,

        [Description("F13")]
        F13 = 0x7C,

        [Description("F14")]
        F14 = 0x7D,

        [Description("F15")]
        F15 = 0x7E,

        [Description("F16")]
        F16 = 0x7F,

        [Description("F17")]
        F17 = 0x80,

        [Description("F18")]
        F18 = 0x81,

        [Description("F19")]
        F19 = 0x82,

        [Description("F20")]
        F20 = 0x83,

        [Description("F21")]
        F21 = 0x84,

        [Description("F22")]
        F22 = 0x85,

        [Description("F23")]
        F23 = 0x86,

        [Description("F24")]
        F24 = 0x87,

        [Description("NUMLOCK")]
        NumLock = 0x90,

        [Description("SCROLL")]
        ScrollLock = 0x91,

        [Description("NECEQ")]
        NEC_Equal = 0x92,

        [Description("FUJJ")]
        Fujitsu_Jisho = 0x92,

        [Description("FUJM")]
        Fujitsu_Masshou = 0x93,

        [Description("FUJT")]
        Fujitsu_Touroku = 0x94,

        [Description("FUJL")]
        Fujitsu_Loya = 0x95,

        [Description("FUJR")]
        Fujitsu_Roya = 0x96,

        [Description("LSHIFT")]
        LeftShift = 0xA0,

        [Description("RSHIFT")]
        RightShift = 0xA1,

        [Description("LCTRL")]
        LeftControl = 0xA2,

        [Description("RCTRL")]
        RightControl = 0xA3,

        [Description("LALT")]
        LeftAlt = 0xA4,

        [Description("RALT")]
        RightAlt = 0xA5,

        [Description("BWBACK")]
        BrowserBack = 0xA6,

        [Description("BWFORWARD")]
        BrowserForward = 0xA7,

        [Description("BWREFRESH")]
        BrowserRefresh = 0xA8,

        [Description("BWSTOP")]
        BrowserStop = 0xA9,

        [Description("BWSEARCH")]
        BrowserSearch = 0xAA,

        [Description("BWFAVORITES")]
        BrowserFavorites = 0xAB,

        [Description("BWHOME")]
        BrowserHome = 0xAC,

        [Description("VOL_MUTE")]
        VolumeMute = 0xAD,

        [Description("VOL_DOWN")]
        VolumeDown = 0xAE,

        [Description("VOL_UP")]
        VolumeUp = 0xAF,

        [Description("MEDIA_NEXT")]
        MediaNextTrack = 0xB0,

        [Description("MEDIA_PREV")]
        MediaPrevTrack = 0xB1,

        [Description("MEDIA_STOP")]
        MediaStop = 0xB2,

        [Description("MEDIA_PLAY")]
        MediaPlayPause = 0xB3,

        [Description("MAIL")]
        LaunchMail = 0xB4,

        [Description("MEDIA_SELECT")]
        LaunchMediaSelect = 0xB5,

        [Description("APP_1")]
        LaunchApplication1 = 0xB6,

        [Description("APP_2")]
        LaunchApplication2 = 0xB7,

        [Description("ATTN")]
        ATTN = 0xF6,

        [Description("CRSEL")]
        CRSel = 0xF7,

        [Description("EXSEL")]
        EXSel = 0xF8,

        [Description("EREOF")]
        EREOF = 0xF9,

        [Description("PLAY")]
        Play = 0xFA,

        [Description("ZOOM")]
        Zoom = 0xFB,

        [Description("NONAME")]
        Noname = 0xFC,

        [Description("XBUTTON_2")]
        XBUTTON_2 = 0xFD,

        [Description("XBUTTON_1")]
        XBUTTON_1 = 0xFE,

        [Description("MOUSE_MIDDLE")]
        MouseWheel = 0xFF,
    }

}
