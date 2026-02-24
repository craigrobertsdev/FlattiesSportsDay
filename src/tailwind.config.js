/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./**/*.{razor,html,cshtml}", "AppConstants.cs"],
    theme: {
        extend: {},
    },
    safelist: [
        "bg-red-500", "text-red-500",
        "bg-blue-400", "text-blue-400",
        "bg-green-400", "text-green-400",
        "bg-yellow-400", "text-yellow-400",
    ],
    plugins: [],
}

