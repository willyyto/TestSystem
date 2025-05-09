import {useState} from "react";
import {Button, Card, CardBody, CardHeader, Checkbox, Input, Spacer} from "@heroui/react";
import {Icon} from "@iconify/react";
import {AnimatePresence, motion} from "framer-motion";
import {useAuth} from "contexts/AuthContext.tsx";

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
    const [agreeToTerms, setAgreeToTerms] = useState(false);
    const toggleVisibility = () => setIsVisible(!isVisible);

    const { login, register } = useAuth();

    const handleAuth = async () => {
        if (isSignUp) {
            if (password !== confirmPassword) {
                alert("Passwords do not match!");
                return;
            }
            if (!agreeToTerms) {
                alert("You must agree to the terms and privacy policy!");
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

    const itemVariants = {
        hidden: {
            opacity: 0,
            y: -10,
        },
        visible: {
            opacity: 1,
            y: 0,
            transition: {
                duration: 0.2,
                ease: "easeInOut",
            },
        },
        exit: {
            opacity: 0,
            y: 10,
            transition: {
                duration: 0.2,
                ease: "easeInOut",
            },
        },
    };

    return (
        <div
            className="flex items-center justify-center min-h-screen bg-cover bg-center"
            style={{ backgroundImage: 'url("https://nextuipro.nyc3.cdn.digitaloceanspaces.com/components-images/black-background-texture-2.jpg")' }}
        >
            <div className="absolute right-10 top-10">
                <div className="flex items-center">
                    <svg fill="none" height="40" viewBox="0 0 32 32" width="40" className="text-white">
                        <path
                            clipRule="evenodd"
                            d="M17.6482 10.1305L15.8785 7.02583L7.02979 22.5499H10.5278L17.6482 10.1305ZM19.8798 14.0457L18.11 17.1983L19.394 19.4511H16.8453L15.1056 22.5499H24.7272L19.8798 14.0457Z"
                            fill="currentColor"
                            fillRule="evenodd"
                        ></path>
                    </svg>
                    <p className="font-medium text-white">TESTSYSTEM</p>
                </div>
            </div>
            <div className="absolute bottom-10 right-10 hidden md:block">
                <p className="max-w-xl text-right text-white/60">
                    <span className="font-medium">“</span>Your ultimate solution for creating, managing, and administering tests seamlessly.<span className="font-medium">”</span>
                </p>
            </div>
            <AnimatePresence mode={"wait"}>
                <motion.div
                    key={isSignUp ? "signUp" : "login"}
                    initial="hidden"
                    animate="visible"
                    exit="exit"
                    className="relative w-full flex items-center justify-center px-8 md:justify-start min-h-screen"
                >
                    <Card radius="sm" fullWidth className="max-w-sm p-4">
                        <CardHeader>
                            <motion.h1 className="text-xl" variants={itemVariants}>
                                {isSignUp ? "Sign Up" : "Log In"}
                            </motion.h1>
                        </CardHeader>
                        <CardBody>
                            <motion.div variants={itemVariants}>
                                <Input
                                    label="Username"
                                    variant="bordered"
                                    placeholder="Enter your username"
                                    size="md"
                                    type="text"
                                    value={username}
                                    onChange={(e) => setUsername(e.target.value)}
                                />
                            </motion.div>
                            <Spacer y={2} />
                            {isSignUp && (
                                <>
                                    <motion.div variants={itemVariants}>
                                        <Input
                                            type="email"
                                            label="Email"
                                            variant="bordered"
                                            placeholder="Enter your email"
                                            size="md"
                                            value={email}
                                            onChange={(e) => setEmail(e.target.value)}
                                        />
                                    </motion.div>
                                    <Spacer y={2} />
                                    <motion.div variants={itemVariants}>
                                        <Input
                                            type="text"
                                            label="Full Name"
                                            variant="bordered"
                                            placeholder="Enter your name"
                                            size="md"
                                            value={name}
                                            onChange={(e) => setName(e.target.value)}
                                        />
                                    </motion.div>
                                    <Spacer y={2} />
                                </>
                            )}
                            <motion.div variants={itemVariants}>
                                <Input
                                    label="Password"
                                    variant="bordered"
                                    placeholder="Enter your password"
                                    endContent={
                                        <button className="focus:outline-none" type="button" onClick={toggleVisibility}>
                                            {isVisible ? (
                                                <Icon icon="solar:eye-closed-bold" className="h-7 w-7 pb-2 text-2xl text-default-400 pointer-events-none" />
                                            ) : (
                                                <Icon icon="solar:eye-bold" className="h-7 w-7 pb-2 text-2xl text-default-400 pointer-events-none" />
                                            )}
                                        </button>
                                    }
                                    size="md"
                                    type={isVisible ? "text" : "password"}
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                            </motion.div>
                            {isSignUp && (
                                <>
                                    <Spacer y={2} />
                                    <motion.div variants={itemVariants}>
                                        <Input
                                            label="Confirm Password"
                                            variant="bordered"
                                            placeholder="Confirm your password"
                                            size="md"
                                            type="password"
                                            value={confirmPassword}
                                            onChange={(e) => setConfirmPassword(e.target.value)}
                                        />
                                    </motion.div>
                                    <Spacer y={2} />
                                    <motion.div variants={itemVariants}>
                                        <label className="block text-sm font-medium text-gray-700">Role</label>
                                        <select
                                            className="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md"
                                            value={role}
                                            onChange={(e) => setRole(e.target.value)}
                                        >
                                            <option value="user">User</option>
                                            <option value="admin">Admin</option>
                                        </select>
                                    </motion.div>
                                    <Spacer y={2} />
                                    <motion.div variants={itemVariants}>
                                        <Checkbox isSelected={agreeToTerms} size="sm" onChange={(e) => setAgreeToTerms(e.target.checked)}>
                                            I agree with the Terms and Privacy Policy
                                        </Checkbox>
                                    </motion.div>
                                </>
                            )}
                            <Spacer y={2} />
                            {!isSignUp && (
                                <motion.div variants={itemVariants} className="flex items-center justify-between mb-4">
                                    <Checkbox isSelected={rememberMe} size="sm" onChange={(e) => setRememberMe(e.target.checked)}>
                                        Remember Me
                                    </Checkbox>
                                    <a href="/forgot-password" className="text-sm text-blue-500">
                                        Forgot Password?
                                    </a>
                                </motion.div>
                            )}
                            <Spacer y={2} />
                            <motion.div variants={itemVariants}>
                                <Button className="w-full" onPress={handleAuth} color="primary">
                                    {isSignUp ? "Sign Up" : "Log In"}
                                </Button>
                            </motion.div>
                            <div className="flex items-center my-4">
                                <motion.hr className="flex-grow border-t border-gray-200 border-b-0" variants={itemVariants} />
                                <motion.span className="mx-4 text-gray-500 text-sm" variants={itemVariants}>OR</motion.span>
                                <motion.hr className="flex-grow border-t border-gray-200 border-b-0" variants={itemVariants} />
                            </div>
                            {isSignUp ? (
                                <motion.div variants={itemVariants}>
                                    <Button className="w-full" variant="bordered" onClick={() => { /* Handle alternative sign-up/login method */ }}>
                                        Continue with Google
                                    </Button>
                                </motion.div>
                            ) : null}
                            <motion.p className="text-center text-sm" variants={itemVariants}>
                                {isSignUp ? (
                                    <>
                                        Already have an account? <a onClick={() => setIsSignUp(false)} className="text-blue-500 cursor-pointer">Log In</a>
                                    </>
                                ) : (
                                    <>
                                        Need to create an account? <a onClick={() => setIsSignUp(true)} className="text-blue-500 cursor-pointer">Sign Up</a>
                                    </>
                                )}
                            </motion.p>
                        </CardBody>
                    </Card>
                </motion.div>
            </AnimatePresence>
        </div>
    );
}
