import { Link } from "@nextui-org/link";
import { button as buttonStyles } from "@nextui-org/theme";
import { subtitle, title } from "components/primitives";

export default function UnAuthorized() {
    return (
        <>
            {/* Unauthorized Section */}
            <section className="flex flex-col items-center justify-center gap-8 pt-8 md:py-56">
                <div className="inline-block max-w-lg text-center justify-center">
                    <h1 className={title()}>Access Denied</h1>
                    <h4 className={subtitle({ class: "mt-4" })}>
                        You do not have the necessary permissions to access this page.
                    </h4>
                </div>

                <div className="flex gap-3">
                    <Link
                        className={buttonStyles({
                            color: "primary",
                            radius: "full",
                            variant: "shadow",
                        })}
                        href="/login"
                    >
                        Go to Login
                    </Link>
                    <Link
                        className={buttonStyles({ variant: "bordered", radius: "full" })}
                        href="/"
                    >
                        Go to Home
                    </Link>
                </div>
            </section>
        </>
    );
}
