/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./**/*.{razor,html,cshtml}", "AppConstants.cs"],
    theme: {
        extend: {},
    },
    safelist: [
        // "bg-red-400",
        // "bg-green-400",
        // "bg-blue-400",
        // "bg-yellow-400",
    ],
    plugins: [],
}

