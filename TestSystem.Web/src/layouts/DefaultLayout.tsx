import Provider from "../Provider.tsx";

export default function DefaultLayout({children}: { children: React.ReactNode; }) {
    return (
        <Provider>
            <div className="relative flex flex-col h-screen">
                    {children}
            </div>
        </Provider>

);
}
