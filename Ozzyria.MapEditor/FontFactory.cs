using SFML.Graphics;

namespace Ozzyria.MapEditor
{
    class FontFactory
    {
        private static Font _regular;
        private static Font _bold;
        private static Font _italic;
        private static Font _boldItalic;

        public static Font GetRegular()
        {
            if(_regular == null)
            {
                _regular = new Font("Fonts\\Bitter-Regular.otf");
            }

            return _regular;
        }

        public static Font GetBold()
        {
            if (_bold == null)
            {
                _bold = new Font("Fonts\\Bitter-Bold.otf");
            }

            return _bold;
        }

        public static Font GetItalic()
        {
            if (_italic == null)
            {
                _italic = new Font("Fonts\\Bitter-Italic.otf");
            }

            return _italic;
        }

        public static Font GetBoldItalic()
        {
            if (_boldItalic == null)
            {
                _boldItalic = new Font("Fonts\\Bitter-BoldItalic.otf");
            }

            return _boldItalic;
        }
    }
}
