/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#333CF5',
          hover: '#282eb5',
        },
        main: '#010218',
        muted: '#757575',
        'hero-bg': '#FAFAFA',
      },
      fontFamily: {
        sans: ['Inter', 'sans-serif'],
        display: ['Poppins', 'sans-serif'],
      },
      animation: {
        'heartbeat': 'heartbeat 5s ease-in-out infinite',
        'float': 'float 3s ease-in-out infinite',
        'bounce-slow': 'bounce 2s infinite',
        'ecg': 'ecg 3s linear infinite', 
        // 🌟 إضافات جديدة لتأثير النبض البطيء والظهور
        'pulse-slow': 'pulse-slow 4s cubic-bezier(0.4, 0, 0.6, 1) infinite',
        'fade-in-up': 'fade-in-up 0.6s ease-out forwards',
        'fade-in': 'fade-in 0.5s ease-out forwards',
      },
      keyframes: {
        heartbeat: {
          '0%, 100%': { transform: 'scale(1)' },
          '50%': { transform: 'scale(1.1)' },
        },
        float: {
          '0%, 100%': { transform: 'translateY(0)' },
          '50%': { transform: 'translateY(-10px)' },
        },
        ecg: {
          '0%': { strokeDashoffset: '1000' },
          '100%': { strokeDashoffset: '0' },
        },
        // 🌟 Keyframes جديدة لتأثير النبض البطيء (Pulse)
        'pulse-slow': {
          '0%, 100%': { transform: 'scale(1)' },
          '50%': { transform: 'scale(1.05)' },
        },
        // 🌟 Keyframes جديدة لتأثير الظهور من الأسفل (Fade In Up)
        'fade-in-up': {
            '0%': { opacity: '0', transform: 'translateY(10px)' },
            '100%': { opacity: '1', transform: 'translateY(0)' },
        },
        // 🌟 Keyframes جديدة لتأثير الظهور العادي (Fade In)
        'fade-in': {
            '0%': { opacity: '0' },
            '100%': { opacity: '1' },
        },
      }
    },
  },
  plugins: [],
}
