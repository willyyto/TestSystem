import {useNavigate} from "react-router-dom";

export default function Page401() {
    const navigate =useNavigate();
    return (
        <>
            {/* Unauthorized Section */}
            <main className="grid min-h-full place-items-center px-6 py-24 sm:py-32 lg:px-8">
                <div className="text-center">
                    <p className="text-base font-semibold text-indigo-600">401</p>
                    <h1 className="mt-4 text-3xl font-bold tracking-tight sm:text-5xl">Unauthorized
                        Access</h1>
                    <p className="mt-6 text-base leading-7">
                        Sorry, you do not have the necessary permissions to view this page.
                    </p>
                    <div className="mt-10 flex items-center justify-center gap-x-6">
                        <a
                            href="#"
                            onClick={navigate("/")}
                            className="rounded-md bg-indigo-600 px-3.5 py-2.5 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
                        >
                            Home
                        </a>
                        <a href="#"
                           onClick={navigate("/login")}
                           className="text-sm font-semibold">
                            Login <span aria-hidden="true">&rarr;</span>
                        </a>
                    </div>
                </div>
            </main>
        </>
    );
}
