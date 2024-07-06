import {Footer} from "components/Footer";
import {Header} from "components/Header";
import Provider from "Provider";

export default function DefaultLayout({
                                          children,
                                      }: {
    children: React.ReactNode;
}) {
    return (
        <Provider>
            <div className="relative flex flex-col h-screen">
                <Header/>
                <main className="container mx-auto max-w-7xl px-6 flex-grow pt-16">
                    {children}
                </main>
                <Footer/>
            </div>
        </Provider>

    );
}
