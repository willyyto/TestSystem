import {Link} from "@nextui-org/link";
import {button as buttonStyles} from "@nextui-org/theme";
import {subtitle, title} from "components/primitives";

export default function Home() {
    return (
        <>
            {/* Hero Section */}
            <section className="flex flex-col items-center justify-center gap-8 pt-8 md:py-56">
                <div className="inline-block max-w-lg text-center justify-center">
                    <h1 className={title()}>Welcome to&nbsp;</h1>
                    <h1 className={title({color: "violet"})}>TestSystem&nbsp;</h1>
                    <br/>
                    <h1 className={title()}>Platform</h1>
                    <h4 className={subtitle({class: "mt-4"})}>
                        Your ultimate solution for creating and taking tests online.
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
                        Get Started
                    </Link>
                    <Link
                        className={buttonStyles({variant: "bordered", radius: "full"})}
                        href="/login"
                    >
                        Login
                    </Link>
                </div>
            </section>
        </>
    );
}
