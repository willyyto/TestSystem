import { useState } from 'react'
import { Link, useNavigate, useLocation } from 'react-router-dom'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import {
    Button,
    Card,
    CardBody,
    CardHeader,
    Checkbox,
    Input,
    Spacer,
    Select,
    SelectItem,
    addToast
} from "@heroui/react"
import { Icon } from "@iconify/react"
import { AnimatePresence, motion } from "framer-motion"
import { useAuth } from '../../../auth/AuthContext.tsx'
import { useMutation } from '@tanstack/react-query'
import { apiMutation } from 'libs/api'
import api from 'libs/api'
import { Helmet } from 'react-helmet-async'

// Schemas
const loginSchema = z.object({
    username: z.string().min(1, 'Username is required'),
    password: z.string().min(1, 'Password is required'),
    rememberMe: z.boolean().optional(),
})

const registerSchema = z.object({
    username: z.string().min(3, 'Username must be at least 3 characters'),
    email: z.string().email('Invalid email address'),
    password: z.string().min(6, 'Password must be at least 6 characters'),
    confirmPassword: z.string(),
    name: z.string().min(2, 'Name must be at least 2 characters'),
    role: z.string().min(1, 'Please select a role'),
}).refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path: ["confirmPassword"],
})

type LoginFormData = z.infer<typeof loginSchema>
type RegisterFormData = z.infer<typeof registerSchema>

interface LoginResponse {
    token: string
    refreshToken: string
}

interface User {
    id: string
    username: string
    name: string
    email: string
    role: string
    company?: any
    isActive: boolean
}

interface RegisterResponse {
    id: string
    message: string
}

const roles = [
    { value: 'User', label: 'User' },
    { value: 'Manager', label: 'Manager' },
    { value: 'Administrator', label: 'Administrator' },
]

export default function Login() {
    const [isVisible, setIsVisible] = useState(false)
    const [isSignUp, setIsSignUp] = useState(false)
    const [showConfirmPassword, setShowConfirmPassword] = useState(false)

    const { login } = useAuth()
    const navigate = useNavigate()
    const location = useLocation()

    const from = location.state?.from?.pathname || '/admin/dashboard'

    const toggleVisibility = () => setIsVisible(!isVisible)

    // Login form
    const {
        register: loginRegister,
        handleSubmit: handleLoginSubmit,
        formState: { errors: loginErrors },
        setError: setLoginError,
        reset: resetLogin
    } = useForm<LoginFormData>({
        resolver: zodResolver(loginSchema),
    })

    // Register form
    const {
        register: registerRegister,
        handleSubmit: handleRegisterSubmit,
        formState: { errors: registerErrors },
        setValue: setRegisterValue,
        watch: watchRegister,
        reset: resetRegister
    } = useForm<RegisterFormData>({
        resolver: zodResolver(registerSchema),
        defaultValues: {
            role: 'User'
        }
    })

    // Login mutation
    const loginMutation = useMutation({
        mutationFn: async (data: LoginFormData) => {
            console.log('Attempting login with:', { username: data.username })

            try {
                const loginResponse = await api.post('/auth/login', {
                    username: data.username,
                    password: data.password,
                })

                console.log('Login response:', loginResponse.data)

                if (!loginResponse.data.success) {
                    throw new Error(loginResponse.data.message || 'Login failed')
                }

                const { token, refreshToken } = loginResponse.data.data as LoginResponse

                if (!token || !refreshToken) {
                    throw new Error('Invalid response: missing tokens')
                }

                const profileResponse = await api.get('/user/profile', {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                })

                console.log('Profile response:', profileResponse.data)

                if (!profileResponse.data.success) {
                    throw new Error(profileResponse.data.message || 'Failed to get user profile')
                }

                const user = profileResponse.data.data as User

                return { user, token, refreshToken }
            } catch (error: any) {
                console.error('Login error:', error)

                if (error.response?.status === 401) {
                    throw new Error('Invalid username or password')
                } else if (error.response?.status === 403) {
                    throw new Error('Account is not active or locked')
                } else if (error.response?.data?.message) {
                    throw new Error(error.response.data.message)
                } else if (error.message) {
                    throw new Error(error.message)
                } else {
                    throw new Error('Login failed. Please try again.')
                }
            }
        },
        onSuccess: ({ user, token, refreshToken }) => {
            console.log('Login successful, updating auth state...')
            login(user, token, refreshToken)

            addToast({title: `Welcome back, ${user.name}!`, color: "success"})
            setTimeout(() => {
                navigate(from, { replace: true })
            }, 100)
        },
        onError: (error: Error) => {
            console.error('Login mutation error:', error)
            addToast({title: error.message, color: "danger"})
            if (error.message.includes('username') || error.message.includes('password')) {
                setLoginError('password', { message: error.message })
            }
        },
    })

    // Register mutation
    const registerMutation = useMutation({
        mutationFn: (data: RegisterFormData) =>
            apiMutation<RegisterResponse>('/auth/register', {
                username: data.username,
                email: data.email,
                password: data.password,
                name: data.name,
                role: data.role,
            }),
        onSuccess: () => {
            addToast({title: 'Registration successful! Please log in.', color: "success"})
            setIsSignUp(false)
            resetRegister()
        },
        onError: (error: any) => {
            addToast({title: error.response?.data?.message || 'Registration failed', color: "danger"})
        },
    })

    const onLoginSubmit = (data: LoginFormData) => {
        console.log('Login form submitted:', data)
        loginMutation.mutate(data)
    }

    const onRegisterSubmit = (data: RegisterFormData) => {
        console.log('Register form submitted:', data)
        registerMutation.mutate(data)
    }

    const handleToggleMode = () => {
        setIsSignUp(!isSignUp)
        resetLogin()
        resetRegister()
    }

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
    }

    return (
        <>
            <Helmet>
                <title>{isSignUp ? 'Register' : 'Login'} - TestSystem</title>
                <meta name="description" content={isSignUp ? 'Create your TestSystem account' : 'Sign in to your TestSystem account'} />
            </Helmet>

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
                            />
                        </svg>
                        <p className="font-medium text-white">TESTSYSTEM</p>
                    </div>
                </div>
                <div className="absolute bottom-10 right-10 hidden md:block">
                    <p className="max-w-xl text-right text-white/60">
                        <span className="font-medium">"</span>Your ultimate solution for creating, managing, and administering tests seamlessly.<span className="font-medium">"</span>
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
                                {!isSignUp ? (
                                    <form onSubmit={handleLoginSubmit(onLoginSubmit)} className="space-y-4">
                                        <motion.div variants={itemVariants}>
                                            <Input
                                                {...loginRegister('username')}
                                                label="Username"
                                                variant="bordered"
                                                placeholder="Enter your username"
                                                size="md"
                                                type="text"
                                                isInvalid={!!loginErrors.username}
                                                errorMessage={loginErrors.username?.message}
                                                autoComplete="username"
                                            />
                                        </motion.div>
                                        <Spacer y={2} />
                                        <motion.div variants={itemVariants}>
                                            <Input
                                                {...loginRegister('password')}
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
                                                isInvalid={!!loginErrors.password}
                                                errorMessage={loginErrors.password?.message}
                                                autoComplete="current-password"
                                            />
                                        </motion.div>
                                        <Spacer y={2} />
                                        <motion.div variants={itemVariants} className="flex items-center justify-between mb-4">
                                            <Checkbox {...loginRegister('rememberMe')} size="sm">
                                                Remember Me
                                            </Checkbox>
                                            <Link to="/auth/forgot-password" className="text-sm text-blue-500">
                                                Forgot Password?
                                            </Link>
                                        </motion.div>
                                        <Spacer y={2} />
                                        <motion.div variants={itemVariants}>
                                            <Button
                                                className="w-full"
                                                type="submit"
                                                color="primary"
                                                isLoading={loginMutation.isPending}
                                                disabled={loginMutation.isPending}
                                            >
                                                {loginMutation.isPending ? 'Signing in...' : 'Log In'}
                                            </Button>
                                        </motion.div>
                                    </form>
                                ) : (
                                    <form onSubmit={handleRegisterSubmit(onRegisterSubmit)} className="space-y-4">
                                        <motion.div variants={itemVariants}>
                                            <Input
                                                {...registerRegister('username')}
                                                label="Username"
                                                variant="bordered"
                                                placeholder="Choose a username"
                                                size="md"
                                                type="text"
                                                isInvalid={!!registerErrors.username}
                                                errorMessage={registerErrors.username?.message}
                                            />
                                        </motion.div>
                                        <Spacer y={2} />
                                        <motion.div variants={itemVariants}>
                                            <Input
                                                {...registerRegister('email')}
                                                type="email"
                                                label="Email"
                                                variant="bordered"
                                                placeholder="Enter your email"
                                                size="md"
                                                isInvalid={!!registerErrors.email}
                                                errorMessage={registerErrors.email?.message}
                                            />
                                        </motion.div>
                                        <Spacer y={2} />
                                        <motion.div variants={itemVariants}>
                                            <Input
                                                {...registerRegister('name')}
                                                type="text"
                                                label="Full Name"
                                                variant="bordered"
                                                placeholder="Enter your full name"
                                                size="md"
                                                isInvalid={!!registerErrors.name}
                                                errorMessage={registerErrors.name?.message}
                                            />
                                        </motion.div>
                                        <Spacer y={2} />
                                        <motion.div variants={itemVariants}>
                                            <Select
                                                label="Role"
                                                placeholder="Select your role"
                                                selectedKeys={watchRegister('role') ? [watchRegister('role')] : []}
                                                onSelectionChange={(keys) => {
                                                    const selectedKey = Array.from(keys)[0] as string
                                                    setRegisterValue('role', selectedKey)
                                                }}
                                                isInvalid={!!registerErrors.role}
                                                errorMessage={registerErrors.role?.message}
                                                variant="bordered"
                                                size="md"
                                            >
                                                {roles.map((role) => (
                                                    <SelectItem key={role.value} value={role.value}>
                                                        {role.label}
                                                    </SelectItem>
                                                ))}
                                            </Select>
                                        </motion.div>
                                        <Spacer y={2} />
                                        <motion.div variants={itemVariants}>
                                            <Input
                                                {...registerRegister('password')}
                                                label="Password"
                                                variant="bordered"
                                                placeholder="Create a password"
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
                                                isInvalid={!!registerErrors.password}
                                                errorMessage={registerErrors.password?.message}
                                            />
                                        </motion.div>
                                        <Spacer y={2} />
                                        <motion.div variants={itemVariants}>
                                            <Input
                                                {...registerRegister('confirmPassword')}
                                                label="Confirm Password"
                                                variant="bordered"
                                                placeholder="Confirm your password"
                                                size="md"
                                                type={showConfirmPassword ? "text" : "password"}
                                                endContent={
                                                    <button className="focus:outline-none" type="button" onClick={() => setShowConfirmPassword(!showConfirmPassword)}>
                                                        {showConfirmPassword ? (
                                                            <Icon icon="solar:eye-closed-bold" className="h-7 w-7 pb-2 text-2xl text-default-400 pointer-events-none" />
                                                        ) : (
                                                            <Icon icon="solar:eye-bold" className="h-7 w-7 pb-2 text-2xl text-default-400 pointer-events-none" />
                                                        )}
                                                    </button>
                                                }
                                                isInvalid={!!registerErrors.confirmPassword}
                                                errorMessage={registerErrors.confirmPassword?.message}
                                            />
                                        </motion.div>
                                        <Spacer y={2} />
                                        <motion.div variants={itemVariants}>
                                            <Button
                                                className="w-full"
                                                type="submit"
                                                color="primary"
                                                isLoading={registerMutation.isPending}
                                                disabled={registerMutation.isPending}
                                            >
                                                {registerMutation.isPending ? 'Creating Account...' : 'Sign Up'}
                                            </Button>
                                        </motion.div>
                                    </form>
                                )}

                                <div className="flex items-center my-4">
                                    <motion.hr className="flex-grow border-t border-gray-200 border-b-0" variants={itemVariants} />
                                    <motion.span className="mx-4 text-gray-500 text-sm" variants={itemVariants}>OR</motion.span>
                                    <motion.hr className="flex-grow border-t border-gray-200 border-b-0" variants={itemVariants} />
                                </div>

                                {isSignUp && (
                                    <motion.div variants={itemVariants}>
                                        <Button className="w-full" variant="bordered" onClick={() => { /* Handle Google sign-up */ }}>
                                            Continue with Google
                                        </Button>
                                    </motion.div>
                                )}

                                <motion.p className="text-center text-sm" variants={itemVariants}>
                                    {isSignUp ? (
                                        <>
                                            Already have an account? <a onClick={handleToggleMode} className="text-blue-500 cursor-pointer">Log In</a>
                                        </>
                                    ) : (
                                        <>
                                            Need to create an account? <a onClick={handleToggleMode} className="text-blue-500 cursor-pointer">Sign Up</a>
                                        </>
                                    )}
                                </motion.p>

                            </CardBody>
                        </Card>
                    </motion.div>
                </AnimatePresence>
            </div>
        </>
    )
}