import {useState} from "react";
import {Button, Card, CardBody, CardHeader, Checkbox, Input, Spacer} from "@nextui-org/react";
import {EyeIcon, EyeSlashIcon} from "@heroicons/react/24/solid";
import {AnimatePresence, motion} from "framer-motion";
import {useAuth} from "contexts/AuthContext";


export default function Login() {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [name, setName] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [rememberMe, setRememberMe] = useState(false);
    const [isVisible, setIsVisible] = useState(false);
    const [isSignUp, setIsSignUp] = useState(false);
    const [role, setRole] = useState("user");
    const toggleVisibility = () => setIsVisible(!isVisible);

    const { login, register } = useAuth();

    const handleAuth = async () => {
        if (isSignUp) {
            if (password !== confirmPassword) {
                alert("Passwords do not match!");
                return;
            }
            try {
                await register(username, password, email, name, role);
                alert("Registration successful!");
                setIsSignUp(false);
            } catch (error) {
                console.error("Registration failed", error);
                alert("Registration failed. Please try again.");
            }
        } else {
            try {
                await login(username, password);
            } catch (error) {
                console.error("Login failed", error);
                alert("Login failed. Please check your credentials and try again.");
            }
        }
    };

    const cardVariants = {
        hidden: {
            opacity: 0,
            rotateY: 90,
        },
        visible: {
            opacity: 1,
            rotateY: 0,
            transition: { duration: 0.2, ease: "easeInOut" },
        },
        exit: {
            opacity: 0,
            rotateY: -90,
            transition: { duration: 0.2, ease: "easeInOut" },
        },
    };

    return (
        <div className="flex items-center justify-center min-h-screen bg-gradient-to-r from-purple-400 via-pink-500 to-red-500">
            <AnimatePresence mode={"wait"}>
                <motion.div
                    key={isSignUp ? "signUp" : "login"}
                    initial="hidden"
                    animate="visible"
                    exit="exit"
                    variants={cardVariants}
                    className="relative w-full"
                >
                    <div className="flex items-center justify-center min-h-screen">
                        <Card radius="sm" fullWidth className="max-w-sm p-4">
                            <CardHeader>
                                <h1 className="text-xl">{isSignUp ? "Sign Up" : "Log In"}</h1>
                            </CardHeader>
                            <CardBody>
                                <Input
                                    label="Username"
                                    variant="bordered"
                                    placeholder="Enter your username"
                                    size="md"
                                    type="text"
                                    value={username}
                                    onChange={(e) => setUsername(e.target.value)}
                                />
                                <Spacer y={2} />
                                {isSignUp && (
                                    <>
                                        <Input
                                            type="email"
                                            label="Email"
                                            variant="bordered"
                                            placeholder="Enter your email"
                                            size="md"
                                            value={email}
                                            onChange={(e) => setEmail(e.target.value)}
                                        />
                                        <Spacer y={2} />
                                        <Input
                                            type="text"
                                            label="Full Name"
                                            variant="bordered"
                                            placeholder="Enter your name"
                                            size="md"
                                            value={name}
                                            onChange={(e) => setName(e.target.value)}
                                        />
                                        <Spacer y={2} />
                                    </>
                                )}
                                <Input
                                    label="Password"
                                    variant="bordered"
                                    placeholder="Enter your password"
                                    endContent={
                                        <button className="focus:outline-none" type="button" onClick={toggleVisibility}>
                                            {isVisible ? (
                                                <EyeSlashIcon
                                                    className="h-7 w-7 pb-2 text-2xl text-default-400 pointer-events-none"/>
                                            ) : (
                                                <EyeIcon
                                                    className="h-7 w-7 pb-2 text-2xl text-default-400 pointer-events-none"/>
                                            )}
                                        </button>
                                    }
                                    size="md"
                                    type={isVisible ? "text" : "password"}
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                                {isSignUp && (
                                    <>
                                        <Spacer y={2} />
                                        <Input
                                            label="Confirm Password"
                                            variant="bordered"
                                            placeholder="Confirm your password"
                                            size="md"
                                            type="password"
                                            value={confirmPassword}
                                            onChange={(e) => setConfirmPassword(e.target.value)}
                                        />
                                        <Spacer y={2} />
                                        <label className="block text-sm font-medium text-gray-700">Role</label>
                                        <select
                                            className="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
                                            value={role}
                                            onChange={(e) => setRole(e.target.value)}
                                        >
                                            <option value="user">User</option>
                                            <option value="admin">Admin</option>
                                        </select>
                                    </>
                                )}
                                <Spacer y={2} />
                                {!isSignUp && (
                                    <div className="flex items-center justify-between mb-4">
                                        <Checkbox isSelected={rememberMe} size="sm" onChange={(e) => setRememberMe(e.target.checked)}>
                                            Remember Me
                                        </Checkbox>

                                        <a href="/forgot-password" className="text-sm text-blue-500">
                                            Forgot Password?
                                        </a>
                                    </div>
                                )}
                                <Spacer y={2} />

                                <Button className="w-full" onPress={handleAuth} color="primary">
                                    {isSignUp ? "Sign Up" : "Log In"}
                                </Button>
                                <div className="flex items-center my-4">
                                    <hr className="flex-grow border-t border-gray-200 border-b-0" />
                                    <span className="mx-4 text-gray-500 text-sm">OR</span>
                                    <hr className="flex-grow border-t border-gray-200 border-b-0" />
                                </div>
                                <Button className="w-full" onClick={() => { /* Handle alternative sign-up/login method */ }}>
                                    Continue with Google
                                </Button>
                                <Spacer y={4} />
                                <p className="text-center text-sm">
                                    {isSignUp ? (
                                        <>
                                            Already have an account? <a onClick={() => setIsSignUp(false)} className="text-blue-500 cursor-pointer">Log In</a>
                                        </>
                                    ) : (
                                        <>
                                            Need to create an account? <a onClick={() => setIsSignUp(true)} className="text-blue-500 cursor-pointer">Sign Up</a>
                                        </>
                                    )}
                                </p>
                            </CardBody>
                        </Card>
                    </div>
                </motion.div>
            </AnimatePresence>
        </div>
    );
}
