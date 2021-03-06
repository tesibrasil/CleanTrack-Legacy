using System;
using System.Runtime.InteropServices;

namespace It.IDnova.Fxw
{
	public class FxwKbd
	{
		private const uint INPUT_KEYBOARD = 1;
		private const int KEY_EXTENDED = 1;
		private const uint KEY_UP = 2;
		private const uint KEY_SCANCODE = 4;

		[DllImport("User32.dll")]
		private static extern uint SendInput(uint numberOfInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] FxwKbd.KEYBOARD_INPUT[] input, int structSize);

		public void press(int vKey)
		{
			this.sendKey(vKey, true);
		}

		public void release(int vKey)
		{
			this.sendKey(vKey, false);
		}

		private void sendKey(int vKey, bool press)
		{
			FxwKbd.KEYBOARD_INPUT[] input = new FxwKbd.KEYBOARD_INPUT[1]
			{
		new FxwKbd.KEYBOARD_INPUT()
			};
			input[0].type = 1U;
			if (press)
			{
				input[0].vk = (ushort)(vKey & (int)byte.MaxValue);
			}
			else
			{
				input[0].vk = (ushort)(vKey & (int)byte.MaxValue);
				input[0].flags |= 2U;
			}
			if ((int)FxwKbd.SendInput(1U, input, Marshal.SizeOf((object)input[0])) != 1)
				throw new Exception("Could not send key: " + (object)vKey);
		}

		public enum KeybEmuMode : byte
		{
			DATA_ONLY,
			DATA_CRLF,
		}

		private struct KEYBOARD_INPUT
		{
			public uint type;
			public ushort vk;
			public uint flags;
		}
	}
}
