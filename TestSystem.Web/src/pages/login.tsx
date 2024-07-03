import { Button } from "@nextui-org/button";
import React from "react";
import { Input } from "@nextui-org/input";
import { Card, CardHeader, CardBody, CardFooter } from "@nextui-org/card";
import { Image } from "@nextui-org/image";

import DefaultLayout from "@/layouts/default";
import { EyeFilledIcon } from "@/components/icons/EyeFilledIcon";
import { EyeSlashFilledIcon } from "@/components/icons/EyeSlashedFilledIcon";


export default function LoginPage() {
  const [isVisible, setIsVisible] = React.useState(false);
  const toggleVisibility = () => setIsVisible(!isVisible);

  return (
    <DefaultLayout>
      <section className="items-center justify-center">
        <Card className="lg:mx-60">
        <CardHeader className="flex gap-3 mx-2">
        <Image
          alt="nextui logo"
          height={40}
          radius="sm"
          src="https://avatars.githubusercontent.com/u/86160567?s=200&v=4"
          width={40}
        />
          <div className="flex flex-col">
            <h1 className="text-xl">Login</h1>
          </div>
        </CardHeader>
        <CardBody className="flex w-full flex-wrap md:flex-nowrap mb-6 md:mb-0 gap-4">
          <Input
            isClearable
            type="text"
            label="Username"
            variant="bordered"
            placeholder="Enter your username"
            className="max-w-s"
          />
          <Input
            className="max-w-s"
            endContent={
              <button
                className="focus:outline-none"
                type="button"
                onClick={toggleVisibility}
              >
                {isVisible ? (
                  <EyeSlashFilledIcon className="text-2xl text-default-400 pointer-events-none" />
                ) : (
                  <EyeFilledIcon className="text-2xl text-default-400 pointer-events-none" />
                )}
              </button>
            }
            label="Password"
            placeholder="Enter your password"
            type={isVisible ? "text" : "password"}
            variant="bordered"
          />
        </CardBody>
        <CardFooter className="items-center justify-center">
        <Button color="primary" type="submit">Login</Button>
        </CardFooter>
      </Card>
      </section>
    </DefaultLayout>
  );
}
